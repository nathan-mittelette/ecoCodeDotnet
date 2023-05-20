// Copyright (c) 2020-2023 nathan-mittelette Co., Ltd.
// This software is released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ecoCodeDotnet.Test.TestData;

public class ecoCodeDotnet
{
    public void Main()
    {

        for (int i = 0; i < 3; i++) // Compliant
        {
            Console.WriteLine(i);
        }

        for (int i = 0; isForEnded(i); i++) // Nocompliant
        {
            Console.WriteLine(i);
        }

    }
    
    public bool isForEnded(int i)
    {
        return i < 3;
    }
}
