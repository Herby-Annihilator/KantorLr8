using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr8.Model.Data
{
	public class DerivativeComparator
	{
		public double Argument { get; set; }
		public double CalculatedDerivativeValue { get; set; }
		public double RealDerivativeValue { get; set; }
		public double Difference { get => Math.Abs
				(CalculatedDerivativeValue - RealDerivativeValue);
		}
		public DerivativeComparator(double arg, double calculatedValue, double realValue)
		{
			Argument = arg;
			CalculatedDerivativeValue = calculatedValue;
			RealDerivativeValue = realValue;
		}
	}
}
