// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Defines a bottom-up rewriter for TSource trees to TTarget trees.
    /// </summary>
    /// <typeparam name="TSource">Type of the source trees. Needs to implement ITree&lt;TSourceNodeType&gt;.</typeparam>
    /// <typeparam name="TSourceNodeType">Node type of the source trees.</typeparam>
    /// <typeparam name="TTarget">Type of the target trees. Needs to implement ITree.</typeparam>
    /// <typeparam name="TWildcardFactory">Wildcard factory type for source tree wildcard nodes.</typeparam>
    public class BottomUpRewriter<TSource, TSourceNodeType, TTarget, TWildcardFactory>
        where TSource : ITree<TSourceNodeType>
        where TTarget : ITree
        where TWildcardFactory : IWildcardFactory<TSource>, new()
    {
        #region Fields

        private readonly TWildcardFactory _wildcardFactory;

        private readonly Dictionary<TSource, int> _forest;
        private readonly Dictionary<TSource, int> _wildcards;
        private readonly Dictionary<TSource, int> _constants;
        private readonly Dictionary<TSourceNodeType, NAryMap<int, int>> _states;
        private readonly Dictionary<Func<TSource, bool>, int> _leafTests;
        private readonly Dictionary<Func<TSource, bool>, int> _fallbackTests;
        private readonly Dictionary<int, RuleBase<TSource, TTarget>> _finals;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new bottom up rewriter instance with no initial rewrite rule definitions.
        /// Collection initializer syntax can be used for the Leaves, Rules, and Fallbacks properties.
        /// </summary>
        public BottomUpRewriter()
            : this(EqualityComparer<TSourceNodeType>.Default)
        {
        }

        /// <summary>
        /// Creates a new bottom up rewriter instance with no initial rewrite rule definitions.
        /// Collection initializer syntax can be used for the Leaves, Rules, and Fallbacks properties.
        /// </summary>
        /// <param name="sourceNodeComparer">Equality comparer used to compare source tree nodes during construction of the rule table and during tree matching.</param>
        public BottomUpRewriter(IEqualityComparer<TSourceNodeType> sourceNodeComparer)
        {
            if (sourceNodeComparer == null)
                throw new ArgumentNullException(nameof(sourceNodeComparer));

            _wildcardFactory = new TWildcardFactory();

            Leaves = new LeafCollection<TSource, TTarget>();
            Leaves.Added += OnLeafAdded;
            Rules = new RuleCollection<TSource, TTarget>();
            Rules.Added += OnRuleAdded;
            Fallbacks = new FallbackCollection<TSource, TTarget>();
            Fallbacks.Added += OnFallbackAdded;

            _forest = new Dictionary<TSource, int>();
            _wildcards = new Dictionary<TSource, int>(new EqualityComparerByType<TSource>());
            _constants = new Dictionary<TSource, int>();
            _states = new Dictionary<TSourceNodeType, NAryMap<int, int>>(sourceNodeComparer);
            _leafTests = new Dictionary<Func<TSource, bool>, int>();
            _fallbackTests = new Dictionary<Func<TSource, bool>, int>();
            _finals = new Dictionary<int, RuleBase<TSource, TTarget>>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a logger object used to log activity in the rewriter.
        /// </summary>
        public TextWriter Log { get; set; }

        /// <summary>
        /// Gets the collection of leaves recognized by the rewriter.
        /// </summary>
        public LeafCollection<TSource, TTarget> Leaves { get; }

        /// <summary>
        /// Gets the collection of rewrite pattern rules recognized by the rewriter.
        /// </summary>
        public RuleCollection<TSource, TTarget> Rules { get; }

        /// <summary>
        /// Gets the collection of fallback rules used by the rewriter.
        /// </summary>
        public FallbackCollection<TSource, TTarget> Fallbacks { get; }

        /// <summary>
        /// Gets a string representation of the internal tables in the rewriter, useful for debugging purposes.
        /// </summary>
        public string DebugView
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine("WILDCARDS:");
                foreach (var w in _wildcards)
                {
                    var wildcard = w.Key;
                    var key = wildcard.ToString();

                    if (wildcard is ITyped typedWildcard)
                        key += " : " + typedWildcard.GetType();

                    sb.AppendLine("  [" + key + ", " + w.Value + "]");
                }
                sb.AppendLine();

                sb.AppendLine("LEAVES:");
                foreach (var l in Leaves)
                    sb.AppendLine("  " + l.ToString());
                sb.AppendLine();

                sb.AppendLine("CONSTANTS:");
                foreach (var c in _constants)
                    sb.AppendLine("  " + c.ToString());
                sb.AppendLine();

                sb.AppendLine("PATTERNS:");
                foreach (var e in _forest)
                    sb.AppendLine("  " + e.ToString());
                sb.AppendLine();

                sb.AppendLine("MAPS:");
                foreach (var kv in _states)
                {
                    sb.AppendLine("  " + kv.Key);
                    foreach (var m in kv.Value)
                        sb.AppendLine("    (" + string.Join(", ", m.Key) + ") --> " + m.Value);
                }
                sb.AppendLine();

                sb.AppendLine("FINAL:");
                foreach (var f in _finals)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "  {0,2} - {1}\n", f.Key, f.Value);
                }
                sb.AppendLine();

                return sb.ToString();
            }
        }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Rewrites the given source tree to a target tree using the rules in the table.
        /// </summary>
        /// <param name="source">Source tree to rewrite.</param>
        /// <returns>Target tree after applying the rewrite rules in the table.</returns>
        public TTarget Rewrite(TSource source)
        {
            WriteLog("INPUT:");
            WriteLog(source.ToString(1));
            WriteLog("");

            var labeled = Label(source);

            WriteLog("LABELED:");
            WriteLog(labeled.ToString(1));
            WriteLog("");

            var res = Reduce(labeled);

            WriteLog("RESULT:");
            WriteLog(res.ToString(1));
            WriteLog("");

            return res;
        }

        #endregion

        #region Protected

        /// <summary>
        /// Writes an entry to the log.
        /// </summary>
        /// <param name="message">Message to log.</param>
        protected void WriteLog(string message)
        {
            Log?.WriteLine(message);
        }

        /// <summary>
        /// Callback from the leaf collection upon addition of a leaf rule.
        /// </summary>
        /// <param name="leaf">Leaf rule that has been added.</param>
        protected virtual void OnLeafAdded(Leaf<TSource, TTarget> leaf)
        {
            if (leaf == null)
                throw new ArgumentNullException(nameof(leaf));

            var state = -(Leaves.Count + 1);
            _leafTests[leaf.InvokePredicate] = state;
            _finals[state] = leaf;
        }

        /// <summary>
        /// Callback from the rule collection upon addition of a rewrite rule.
        /// </summary>
        /// <param name="rule">Rewrite rule that has been added.</param>
        protected virtual void OnRuleAdded(Rule<TSource, TTarget> rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            var wildcards = (from hole in rule.Pattern.Parameters
                             select _wildcardFactory.CreateWildcard(hole))
                            .ToArray();

            var pattern = rule.GetPattern(wildcards);

            var state = GetState(pattern, node => wildcards.Any(wildcard => ReferenceEquals(node, wildcard)));
            _finals[state.Value] = rule;

            var paths = new List<WildcardTraversal<TSource>>();
            var traversals = state.WildcardTraversals;
            foreach (var wildcard in wildcards)
            {
                if (!traversals.TryGetValue(wildcard, out WildcardTraversal<TSource> path))
                    throw new InvalidOperationException("Unused wildcard \"" + wildcard + "\" in pattern \"" + rule.Pattern + "\".");

                paths.Add(path);
            }

            rule.SetWildcardTraversalPaths(paths);
        }

        /// <summary>
        /// Callback from the fallback collection upon addition of a fallback rule.
        /// </summary>
        /// <param name="fallback">Fallback rule that has been added.</param>
        protected virtual void OnFallbackAdded(Fallback<TSource, TTarget> fallback)
        {
            if (fallback == null)
                throw new ArgumentNullException(nameof(fallback));

            var state = int.MaxValue - Fallbacks.Count + 1;
            _fallbackTests[fallback.InvokePredicate] = state;
            _finals[state] = fallback;
        }

        #endregion

        #region Private

        #region Setup implementation

        private State GetState(TSource node, Func<TSource, bool> isWildcard)
        {
            if (isWildcard(node))
            {
                if (!_wildcards.TryGetValue(node, out int state))
                {
                    state = AddToForest(node);
                    _wildcards[node] = state;
                }

                var res = new State(state);
                res.WildcardTraversals[node] = new WildcardTraversal<TSource>(node);
                return res;
            }
            else
            {
                var childStates = new List<State>();
                var wildcardTraversals = new WildcardTraversalMap<TSource>();
                var childIndex = 0;
                foreach (var childNode in node.Children)
                {
                    var childState = GetState((TSource)childNode, isWildcard);
                    childStates.Add(childState);

                    var childTraversals = childState.WildcardTraversals;
                    childTraversals.PushPathSegment(childIndex);
                    wildcardTraversals.Merge(childTraversals);

                    childIndex++;
                }

                if (childStates.Count == 0)
                {
                    return GetStateForConstant(node);
                }

                if (!_states.TryGetValue(node.Value, out NAryMap<int, int> map))
                {
                    map = new NAryMap<int, int>(childStates.Count);
                    _states[node.Value] = map;
                }

                var childStateValues = childStates.Select(c => c.Value).ToList();

                if (!map.TryGetValue(childStateValues, out int state))
                {
                    state = AddToForest(node);
                    map[childStateValues] = state;
                }

                var res = new State(state, wildcardTraversals);
                return res;
            }
        }

        private State GetStateForConstant(TSource tree)
        {
            if (!_constants.TryGetValue(tree, out int res))
            {
                res = AddToForest(tree);
                _constants[tree] = res;
            }

            return new State(res);
        }

        private int AddToForest(TSource tree)
        {
            int res = _forest.Count;
            _forest[tree] = res;
            return res;
        }

        private sealed class State
        {
            public State(int value)
                : this(value, new WildcardTraversalMap<TSource>())
            {
            }

            public State(int state, WildcardTraversalMap<TSource> wildcardTraversals)
            {
                Value = state;
                WildcardTraversals = wildcardTraversals;
            }

            public int Value { get; }

            public WildcardTraversalMap<TSource> WildcardTraversals { get; }
        }

        #endregion

        #region Rewrite implementation

        private LabeledTree<TSourceNodeType> Label(TSource tree) => LabelCore(tree);

        private LabeledTree<TSourceNodeType> LabelCore(ITree<TSourceNodeType> node)
        {
            var matches = GetWildcardMatches(node).ToList();

            var label = new Label<TSourceNodeType>(node, matches);
            var res = new LabeledTree<TSourceNodeType>(label, node.Children.Select(childNode => LabelCore(childNode)));

            if (res.Children.Count == 0)
            {
                var sourceNode = (TSource)node;

                if (_constants.TryGetValue(sourceNode, out int constantState))
                {
                    matches.Add(GetMatch(constantState, res));
                }

                foreach (var leafTest in _leafTests)
                {
                    if (leafTest.Key(sourceNode))
                    {
                        var leafState = leafTest.Value;
                        matches.Add(GetMatch(leafState, res));
                    }
                }
            }
            else
            {
                // TODO: How should we deal with the concept of "base definitions"?
                //       What about string.Equals(string) match against string.Equals(object)?
                //       Contravariance for signatures based on child nodes and a property of the tree node?
                if (_states.TryGetValue(node.Value, out NAryMap<int, int> operatorStateMap) && res.Children.Count == operatorStateMap.Levels)
                {
                    var maps = new List<NAryMapOrLeaf<int, int>>
                    {
                        NAryMapOrLeaf<int, int>.CreateMap(operatorStateMap)
                    };

                    //
                    // This is the core of the labeling algorithm. We scan the recursively labeled children
                    // (which correspond to the operator's operands) one-by-one, trying to make progress in
                    // the state map for the operator by indexing one operand state at a time.
                    //
                    // E.g.  [1,3] (op) [2,4,7]
                    //
                    //       Given the state transition map of operator (op), we index into the map using the
                    //       matching states associated with the first operand, i.e. [1,3]. This will give us
                    //       at most two new maps, resembling partial application of (op) with only the first
                    //       operand. Next, we index into those maps using the second child's matching states
                    //       (i.e. [2,4,7]) which gives us the next (and final "leaf") level of maps.
                    //
                    foreach (var labeledChild in res.Children)
                    {
                        var newMap = new List<NAryMapOrLeaf<int, int>>();

                        //
                        // All of the possible states for current child (operand) need to be attempted in the
                        // current maps. This gives us the next level of maps.
                        //
                        var labels = labeledChild.Value.Labels;
                        foreach (var childLabel in labels)
                        {
                            foreach (var map in maps)
                            {
                                if (map.Map.TryGetValue(childLabel.State, out NAryMapOrLeaf<int, int> nxt))
                                {
                                    newMap.Add(nxt);
                                }
                            }
                        }

                        maps = newMap;
                    }

                    foreach (var map in maps)
                    {
                        matches.Add(GetMatch(map.Leaf, res));
                    }
                }
            }

            //
            // If a node is assignable to a wildcard but has no available reduction, we can try the
            // fallback strategy of local evaluation by funcletizing the remainder tree.
            //
            if (ShouldConsiderFallback(matches))
            {
                var sourceNode = (TSource)node;

                foreach (var fallbackTest in _fallbackTests)
                {
                    if (fallbackTest.Key(sourceNode))
                    {
                        var fallbackState = fallbackTest.Value;
                        matches.Add(GetMatch(fallbackState, res));
                    }
                }
            }

            return res;
        }

        private static bool ShouldConsiderFallback(List<Match> matches)
        {
            // if (matches.Any(match => match is Match.Wildcard) && !matches.Any(match => match is Match.Final))

            var hasWildcard = false;
            var hasFinal = false;
            foreach (var match in matches)
            {
                if (!hasWildcard)
                {
                    if (match is Match.Wildcard)
                    {
                        hasWildcard = true;
                        if (hasFinal)
                        {
                            break;
                        }
                    }
                }

                if (!hasFinal)
                {
                    if (match is Match.Final)
                    {
                        hasFinal = true;
                        if (hasWildcard)
                        {
                            break;
                        }
                    }
                }
            }

            return hasWildcard && !hasFinal;
        }

        private Match GetMatch(int state, LabeledTree<TSourceNodeType> self)
        {
            if (_finals.TryGetValue(state, out RuleBase<TSource, TTarget> rule))
            {
                var cost = rule.Cost;

                foreach (var wildcardBinding in rule.RecurseOnWildcards(self))
                {
                    //
                    // Basic consistency check below. Making sure the fact the rule led us to bind
                    // a wildcard originates from indeed having a wildcard label.
                    //
                    // Note: To check state table consistency, we could check whether the labeled
                    //       tree (wildcardBinding.Tree.Value.Tree) can be assigned to any of
                    //       the wildcards, using an Any predicate. (For debugging only.)
                    //
                    var labels = wildcardBinding.Tree.Value.Labels;
                    Invariant.Assert(labels.OfType<Match.Wildcard>().Any(), "No assignable wildcard found.");

                    //
                    // In case of optimizers we can be faced with partial coverings. The reduction
                    // phase will make sure we can cover the whole tree if needed.
                    //
                    var cheapest = labels.OfType<Match.Final>().OrderBy(final => final.Cost).FirstOrDefault();
                    if (cheapest != null)
                    {
                        cost += cheapest.Cost;
                    }
                }

                return new Match.Final(state, cost);
            }

            return new Match(state);
        }

        private IEnumerable<Match> GetWildcardMatches(ITree<TSourceNodeType> tree)
        {
            var treeType = tree is ITyped typedTree ? typedTree.GetType() : Types.Top;

            foreach (var wildcardState in _wildcards)
            {
                var wildcard = wildcardState.Key;

                var wildcardType = wildcard is ITyped typedWildcard ? typedWildcard.GetType() : Types.Top;
                if (treeType.IsAssignableTo(wildcardType))
                    yield return new Match.Wildcard(wildcardState.Value);
            }
        }

        private TTarget Reduce(LabeledTree<TSourceNodeType> labeled) => ReduceCore(labeled);

        private TTarget ReduceCore(ITree<Label<TSourceNodeType>> labeled)
        {
            var cheapest = labeled.Value.Labels.OfType<Match.Final>().OrderBy(l => l.Cost).FirstOrDefault();
            if (cheapest == null)
            {
                var tree = labeled.Value.Tree;
                var isOptimizing = tree is TTarget;
                if (!isOptimizing)
                    throw new InvalidOperationException("Irreducible node found: " + tree.ToString());

                //
                // An optimizer can take baby steps down the subject tree, till it finds a reducible node.
                //
                var optimizingSourceTree = (TTarget)labeled.Value.Tree;
                var childNodes = labeled.Children.Select(ReduceCore).Cast<ITree>().ToList();
                return (TTarget)optimizingSourceTree.Update(childNodes);
            }

            var rule = _finals[cheapest.State];

            if (rule is Leaf<TSource, TTarget> leafRule)
            {
                return leafRule.InvokeConvert((TSource)labeled.Value.Tree);
            }
            else
            {
                if (rule is Fallback<TSource, TTarget> fallbackRule)
                {
                    return fallbackRule.InvokeConvert((TSource)labeled.Value.Tree);
                }
                else
                {
                    var goalArguments = (from wildcardBinding in rule.RecurseOnWildcards(labeled)
                                         select ReduceCore(wildcardBinding.Tree))
                                        .ToArray();

                    return ((Rule<TSource, TTarget>)rule).InvokeGoal(goalArguments);
                }
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
