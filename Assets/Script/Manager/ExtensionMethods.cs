using System.Collections.Generic;
using System.Linq;
public static class ExtensionMethods
{
    private static System.Random rng = new System.Random();  
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
    
    public static float[] BotPlayerBalance = { 15995f, 11225f, 10000f, 50000f, 85624f, 96545f, 65826f, 94584f, 72665f, 38561f };
    
}