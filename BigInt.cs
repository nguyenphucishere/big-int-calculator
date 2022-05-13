using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bigIntCalculator
{
    class BigInt
    {
        private StringBuilder value;

        public StringBuilder Value { get => value; set => this.value = value; }

        public BigInt()
        {
            Value = new StringBuilder();
        }

        public BigInt(int number)
        {
            Value = new StringBuilder(number.ToString());
        }

        public BigInt(double number)
        {
            Value = new StringBuilder(number.ToString());
        }

        public BigInt(string number)
        {
            Value = new StringBuilder(number);
        }

        public BigInt(BigInt bigInt)
        {
            Value = new StringBuilder(bigInt.ToString());
        }

        public static BigInt operator +(BigInt bigInt) => bigInt;
        public static BigInt operator -(BigInt bigInt) => bigInt.OppositeNumber();
        public static BigInt operator ++(BigInt bigInt) => bigInt += 1;
        public static BigInt operator --(BigInt bigInt) => bigInt -= 1;

        private static string getPadLeftString(BigInt bigInt, int maxLength) => 
            String.Empty.PadLeft(maxLength - bigInt.Value.Length, '0');
        
        
        public static void padLeftString(BigInt bigInt, string padLeftString)
        {
            bigInt.Value.Insert(0, padLeftString);
        }
        
        public static bool operator <(BigInt a, BigInt b) => IsNegativeNumber(a - b);
        public static bool operator <(BigInt a, int b) => a < new BigInt(b);

        
        public static bool operator >(BigInt a, BigInt b) => !IsNegativeNumber(a - b);
        public static bool operator >(BigInt a, int b) => a > new BigInt(b);

        public static bool operator >=(BigInt a, BigInt b) => a == b || a > b;
        public static bool operator >=(BigInt a, int b) => a == b || a > b;


        public static bool operator <=(BigInt a, BigInt b) => a == b || a < b;
        public static bool operator <=(BigInt a, int b) => a == b || a < b;


        public static bool operator ==(BigInt a, BigInt b) => a.Value == b.Value;
        public static bool operator ==(BigInt a, int b) => a == new BigInt(b);

        public static bool operator !=(BigInt a, BigInt b) => !(a == b);
        public static bool operator !=(BigInt a, int b) => !(a == new BigInt(b));

        public static BigInt operator +(BigInt a, BigInt b)
        {
            BigInt newA = new BigInt(a);
            BigInt newB = new BigInt(b);

            /*                         _
               (-a) + (-b) => (-a) - b  |
                                        | => same
               a + (-b) => a - b       _|
            */
            if(IsNegativeNumber(a) && IsNegativeNumber(b) || IsNegativeNumber(b)) return a - -b;

            // (-a) + b => b - a
            if (IsNegativeNumber(a)) return b - a;

            int highestLength = Math.Max(newA.Value.Length, newB.Value.Length);

            padLeftString(newA, getPadLeftString(newA, highestLength));     
            padLeftString(newB, getPadLeftString(newB, highestLength));

            int temp = 0;
            BigInt total = new BigInt();
            
            for(int i = highestLength - 1; i >= 0; i--)
            {
                int intA = int.Parse(newA.Value[i].ToString());
                int intB = int.Parse(newB.Value[i].ToString());
                int result = Math.Abs(intA + intB + temp);


                total.Value.Insert(0, i == 0 ? result.ToString() : (result % 10).ToString());

                temp = result / 10;
            }

            return total;
        }
        public static BigInt operator +(BigInt a, int b) => a + new BigInt(b);


        public static BigInt operator -(BigInt a, BigInt b)
        {
            BigInt newA = new BigInt(a);
            BigInt newB = new BigInt(b);

            //use - for a oppsoite number of -(-a) = a

            // (-a) - (-b) => -(a - b)
            if (IsNegativeNumber(a) && IsNegativeNumber(b)) 
                return -(-a - -b);

            if (IsNegativeNumber(b)) // a - (-b) => a + b
                return a + -b;

            if (IsNegativeNumber(a)) // (-a) - b => -(a + b)
                return -(-a + b);

            // a - b => (-b) + a => -(b - a)
            if (a < b) return -(b - a);

            int highestLength = Math.Max(newA.Value.Length, newB.Value.Length);

            padLeftString(newA, getPadLeftString(newA, highestLength));
            padLeftString(newB, getPadLeftString(newB, highestLength));

            int temp = 0;
            BigInt result = new BigInt();

            for (int i = highestLength - 1; i >= 0; i--)
            {
                int intA = int.Parse(newA.Value[i].ToString());
                int intB = int.Parse(newB.Value[i].ToString());
                int temp2 =  temp;
                
                temp = Convert.ToInt32(intA < intB);

                result.Value.Insert(0, Math.Abs(intA + temp*10 - intB - temp2));
            }

            return result;
        }
        public static BigInt operator -(BigInt a, int b) => a + new BigInt(b);


        public static BigInt operator *(BigInt a, BigInt b)
        {
            // a * (-b) => -(b * a)
            // (-a) * b => b * (-a) => this mean b now becomes a, and a becomes b
            if(IsNegativeNumber(a) || IsNegativeNumber(b)) return -(a * b);

            // (-a) * (-b) => a * b
            if (IsNegativeNumber(a) && IsNegativeNumber(b)) return -a * -b;

            int temp = 0;

            BigInt sum = new BigInt(0);

            for (int i = b.Value.Length - 1; i >= 0; i--)
            {
                int intB = int.Parse(b.Value[i].ToString());

                BigInt result = new BigInt();

                for (int j = b.Value.Length - 1; j >= 0; j--)
                {
                    int intA = int.Parse(b.Value[j].ToString());

                    int multiple = Math.Abs(intA * intB + temp);
                    
                    result.Value.Insert(0, j == 0 ? multiple.ToString() : (multiple % 10).ToString());

                    temp = multiple / 10;
                }

                result.Value.Append('0', b.Value.Length - i - 1);
                sum += result;
            }

            return sum;
        }
        public static BigInt operator *(BigInt a, int b) =>  a * new BigInt(b);


        //NOTE: THINK TWICE BEFORE RUN UNLESS YOU HAVE TIME (SLOW ALGORITHM)
        #region Beta Operator Function
        public static BigInt operator /(BigInt a, BigInt b)
        {

            /*it is so hard to find the fast algorithm so I tried this: 
                https://en.wikipedia.org/wiki/Division_algorithm
             */

            BigInt Q = new BigInt(0);
            BigInt R = new BigInt(a);

            if (b == 0) throw new DivideByZeroException();


            if (b < 0) return -(a / b);
            if (a < 0)
            {
                Q = -a / b;
                R = -a % b;

                if (R == 0) return -Q;
                else return -Q - 1;
            }        

            while (R >= b) 
            {
                Q++;
                R -= b;
            }

            return Q;
        }
        public static BigInt operator %(BigInt a, BigInt b)
        {

            BigInt Q = new BigInt(0);
            BigInt R = new BigInt(a);

            if (b == 0) throw new DivideByZeroException();
            if (b < 0) return -(a / b);
            if (a < 0)
            {
                R = -a % b;

                if (R == 0) return R;
                else return b - R;
            }

            while (R >= b)
            {
                Q++;
                R -= b;
            }

            return R;
        }

        #endregion

        public override string ToString()
        {
            return Value.ToString();
        }

        public BigInt OppositeNumber()
        {
            BigInt oppositeNumber = new BigInt(Value.ToString());
            if (IsNegativeNumber(oppositeNumber))
            {
                oppositeNumber.Value.Remove(0, 1);
            }
            else
            {
                oppositeNumber.Value.Insert(0, "-");
            }

            return oppositeNumber;
        }

        public static bool IsNegativeNumber(BigInt bigInt)
        {
            return bigInt.Value[0] == '-';
        }

        public BigInt GetAbsoluteValue()
        {
            BigInt abs = new BigInt(this);
            if (IsNegativeNumber(abs))
            {
                abs.Value.Remove(0, 1);
            }

            return abs;
        }

        public override bool Equals(object obj)
        {
            return this == (BigInt)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
