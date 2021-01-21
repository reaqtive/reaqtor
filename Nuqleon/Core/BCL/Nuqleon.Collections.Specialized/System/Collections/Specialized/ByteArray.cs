// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Collections.Specialized
{
    internal struct ByteArray1 : IByteArray
    {
        private byte b0;

        public int Length => 1;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray2 : IByteArray
    {
        private byte b0;
        private byte b1;

        public int Length => 2;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray3 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;

        public int Length => 3;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray4 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;
        private byte b3;

        public int Length => 4;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                3 => b3,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    case 3:
                        b3 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray5 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;

        public int Length => 5;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                3 => b3,
                4 => b4,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    case 3:
                        b3 = value;
                        return;
                    case 4:
                        b4 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray6 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;
        private byte b5;

        public int Length => 6;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                3 => b3,
                4 => b4,
                5 => b5,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    case 3:
                        b3 = value;
                        return;
                    case 4:
                        b4 = value;
                        return;
                    case 5:
                        b5 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray7 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;
        private byte b5;
        private byte b6;

        public int Length => 7;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                3 => b3,
                4 => b4,
                5 => b5,
                6 => b6,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    case 3:
                        b3 = value;
                        return;
                    case 4:
                        b4 = value;
                        return;
                    case 5:
                        b5 = value;
                        return;
                    case 6:
                        b6 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray8 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;
        private byte b5;
        private byte b6;
        private byte b7;

        public int Length => 8;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                3 => b3,
                4 => b4,
                5 => b5,
                6 => b6,
                7 => b7,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    case 3:
                        b3 = value;
                        return;
                    case 4:
                        b4 = value;
                        return;
                    case 5:
                        b5 = value;
                        return;
                    case 6:
                        b6 = value;
                        return;
                    case 7:
                        b7 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray9 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;
        private byte b5;
        private byte b6;
        private byte b7;
        private byte b8;

        public int Length => 9;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                3 => b3,
                4 => b4,
                5 => b5,
                6 => b6,
                7 => b7,
                8 => b8,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    case 3:
                        b3 = value;
                        return;
                    case 4:
                        b4 = value;
                        return;
                    case 5:
                        b5 = value;
                        return;
                    case 6:
                        b6 = value;
                        return;
                    case 7:
                        b7 = value;
                        return;
                    case 8:
                        b8 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray10 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;
        private byte b5;
        private byte b6;
        private byte b7;
        private byte b8;
        private byte b9;

        public int Length => 10;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                3 => b3,
                4 => b4,
                5 => b5,
                6 => b6,
                7 => b7,
                8 => b8,
                9 => b9,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    case 3:
                        b3 = value;
                        return;
                    case 4:
                        b4 = value;
                        return;
                    case 5:
                        b5 = value;
                        return;
                    case 6:
                        b6 = value;
                        return;
                    case 7:
                        b7 = value;
                        return;
                    case 8:
                        b8 = value;
                        return;
                    case 9:
                        b9 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal struct ByteArray11 : IByteArray
    {
        private byte b0;
        private byte b1;
        private byte b2;
        private byte b3;
        private byte b4;
        private byte b5;
        private byte b6;
        private byte b7;
        private byte b8;
        private byte b9;
        private byte b10;

        public int Length => 11;

        public byte this[int index]
        {
            get => index switch
            {
                0 => b0,
                1 => b1,
                2 => b2,
                3 => b3,
                4 => b4,
                5 => b5,
                6 => b6,
                7 => b7,
                8 => b8,
                9 => b9,
                10 => b10,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            set
            {
                switch (index)
                {
                    case 0:
                        b0 = value;
                        return;
                    case 1:
                        b1 = value;
                        return;
                    case 2:
                        b2 = value;
                        return;
                    case 3:
                        b3 = value;
                        return;
                    case 4:
                        b4 = value;
                        return;
                    case 5:
                        b5 = value;
                        return;
                    case 6:
                        b6 = value;
                        return;
                    case 7:
                        b7 = value;
                        return;
                    case 8:
                        b8 = value;
                        return;
                    case 9:
                        b9 = value;
                        return;
                    case 10:
                        b10 = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

}
