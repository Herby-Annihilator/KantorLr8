using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KantorLr8.Model.Data
{
	public class Point : INotifyPropertyChanged
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
