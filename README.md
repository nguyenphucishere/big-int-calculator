### Introduction to BigInt

- __`BigInt`__ enables programming language to calculate large and big number using __String__.
- The limit of the result is __1,073,741,823 characters on 64-bit machine__ (theoretical limit is 2,147,483,647 characters)

#### BigInt + BigInt
```c#
   BigInt a = new BigInt("123456789");
   BigInt b = new BigInt("123456");
   Console.WriteLine(a + b);
```

#### BigInt - BigInt
```c#
   BigInt a = new BigInt("123456789");
   BigInt b = new BigInt("123456");
   Console.WriteLine(a - b);
```

#### BigInt * BigInt
```c#
   BigInt a = new BigInt("123456789");
   BigInt b = new BigInt("123456");
   Console.WriteLine(a * b);
```

#### BigInt / BigInt
```c#
   BigInt a = new BigInt("123456789");
   BigInt b = new BigInt("123456");
   Console.WriteLine(a / b);
```
NOTE: This feature may be slow due to its algorithm[^1]

#### BigInt Operation
```c#
   BigInt a = new BigInt("123456789");
   BigInt b = new BigInt("123456");
   
   a < b; //false
   a <= b; //false
   
   a > b; //true
   a >= b; //true
   
   a == b; //false
   a != b; //true
   
   a++; // a = 123456790
   b++; // b = 123457
   
   a--; // a = 123456788
   b--; // b = 123455

```

[^1]: https://en.wikipedia.org/wiki/Division_algorithm
