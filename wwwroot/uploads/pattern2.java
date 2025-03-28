// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using System;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        int count=10;
        for(int i=4;i>=1;i--){
            if(i%2==0){
                for(int j=i;j>=1;j--){
                    Console.Write(count--+" ");
                }
            }else{
                count=count-(i-1);
                for(int j=i;j>=1;j--){
                    Console.Write(count+++" ");
                }
                count=count-(i+1);
            }
            Console.WriteLine();
        }
    }
}