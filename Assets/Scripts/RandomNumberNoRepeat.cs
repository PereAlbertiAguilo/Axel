using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomNumberNoRepeat
{
    public static int GetRandomNumberFromList(List<int> usedNumbers, int min, int max)
    {
        int randomNumber = Random.Range(min, max);

        int result;

        if(usedNumbers.Count >= max)
        {
            usedNumbers.Clear();
        }

        if(!usedNumbers.Contains(randomNumber))
        {
            result = randomNumber;
        }
        else
        {
            while(usedNumbers.Contains(randomNumber))
            {
                randomNumber = Random.Range(min, max);
            }

            result = randomNumber;
        }

        return result;
    }
}
