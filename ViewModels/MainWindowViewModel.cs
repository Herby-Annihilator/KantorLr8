using KantorLr8.Infrastructure.Commands;
using KantorLr8.Model.Data;
using KantorLr8.ViewModels.Base;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Markup;

namespace KantorLr8.ViewModels
{
	[MarkupExtensionReturnType(typeof(MainWindowViewModel))]
	public class MainWindowViewModel : ViewModel
	{
		private Action selectedDerivativeFunction;

		private Point _firstPoint;
		private Point secondPoint;
		private Point _penultimatePoint;
		private Point _lastPoint;


		public MainWindowViewModel()
		{
			selectedDerivativeFunction = CalculateFirstDerivative;
			SelectCalculatingDeriavtiveCommand = new LambdaCommand(OnSelectCalculatingDeriavtiveCommandExecuted, CanSelectCalculatingDeriavtiveCommandExecute);
			RemoveSelectedPointInFunctionTableCommand = new LambdaCommand(OnRemoveSelectedPointInFunctionTableCommandExecuted, CanRemoveSelectedPointInFunctionTableCommandExecute);
			AddNewPointInFunctionTableCommand = new LambdaCommand(OnAddNewPointInFunctionTableCommandExecuted, CanAddNewPointInFunctionTableCommandxecute);
			GenerateFunctionTableCommand = new LambdaCommand(OnGenerateFunctionTableCommandExecuted, CanGenerateFunctionTableCommandxecute);
			CalculateDerivativeCommand = new LambdaCommand(OnCalculateDerivativeCommandExecuted, CanCalculateDerivativeCommandxecute);
		}

		#region Properties
		private string _title = "Title";
		public string Title { get => _title; set => Set(ref _title, value); }

		private string _status = "Численное дифференцирование";
		public string Status { get => _status; set => Set(ref _status, value); }

		private string _functionString;
		public string FunctionString { get => _functionString; set => Set(ref _functionString, value); }

		private string _functionFirstDerivativeString;
		public string FunctionFirstDerivativeString { get => _functionFirstDerivativeString; set => Set(ref _functionFirstDerivativeString, value); }

		private string _functionSecondDerivativeString;
		public string FunctionSecondDerivativeString { get => _functionSecondDerivativeString; set => Set(ref _functionSecondDerivativeString, value); }

		private string _functionThirdDerivativeString;
		public string FunctionThirdDerivativeString { get => _functionThirdDerivativeString; set => Set(ref _functionThirdDerivativeString, value); }

		private string _leftBoardText;
		public string LeftBoardText { get => _leftBoardText; set => Set(ref _leftBoardText, value); }

		private string _rightBoardText;
		public string RightBoardText { get => _rightBoardText; set => Set(ref _rightBoardText, value); }

		private string _stepdText;
		public string StepdText { get => _stepdText; set => Set(ref _stepdText, value); }

		private string _disturbance;
		public string Disturbance { get => _disturbance; set => Set(ref _disturbance, value); }

		public ObservableCollection<Point> FunctionTable { get; set; } = new ObservableCollection<Point>();
		public ObservableCollection<DerivativeComparator> DerivativeTable { get; set; } = new ObservableCollection<DerivativeComparator>();

		private Point _selectedPointInFunctionTable;
		public Point SelectedPointInFunctionTable { get => _selectedPointInFunctionTable; set => Set(ref _selectedPointInFunctionTable, value); }
		#endregion

		#region Commands
		public ICommand SelectCalculatingDeriavtiveCommand { get; }
		private void OnSelectCalculatingDeriavtiveCommandExecuted(object p)
		{
			if (p.ToString() == "1")
				selectedDerivativeFunction = CalculateFirstDerivative;
			else if (p.ToString() == "2")
				selectedDerivativeFunction = CalculateSecondDerivative;
			else
				selectedDerivativeFunction = CalculateThirdDerivative;
		}
		private bool CanSelectCalculatingDeriavtiveCommandExecute(object p) => true;

		public ICommand RemoveSelectedPointInFunctionTableCommand { get; }
		private void OnRemoveSelectedPointInFunctionTableCommandExecuted(object p)
		{
			try
			{
				FunctionTable.Remove(SelectedPointInFunctionTable);
				SelectedPointInFunctionTable = null;
				Status = $"Точка удалена";
			}
			catch (Exception e)
			{
				Status = $"Опреация провалена. Причина: {e.Message}";
			}
		}
		private bool CanRemoveSelectedPointInFunctionTableCommandExecute(object p)
		{
			return SelectedPointInFunctionTable != null;
		}

		public ICommand AddNewPointInFunctionTableCommand { get; }
		private void OnAddNewPointInFunctionTableCommandExecuted(object p)
		{
			try
			{
				FunctionTable.Add(new Point(0, 0));
				Status = $"Строка добавлена";
			}
			catch (Exception e)
			{
				Status = $"Опреация провалена. Причина: {e.Message}";
			}
		}
		private bool CanAddNewPointInFunctionTableCommandxecute(object p)
		{
			return true;
		}

		public ICommand GenerateFunctionTableCommand { get; }
		private void OnGenerateFunctionTableCommandExecuted(object p)
		{
			try
			{
				Function function = new Function(FunctionString);
				Expression expression;
				double left = Convert.ToDouble(LeftBoardText);
				double right = Convert.ToDouble(RightBoardText);
				double step = Convert.ToDouble(StepdText);
				FunctionTable.Clear();
				expression = new Expression($"f({(left - step * 2).ToString().Replace(",", ".")})", function);
				_firstPoint = new Point(left - step * 2, expression.calculate());

				expression = new Expression($"f({(left - step).ToString().Replace(",", ".")})", function);
				secondPoint = new Point(left - step, expression.calculate());

				expression = new Expression($"f({(right + step).ToString().Replace(",", ".")})", function);
				_penultimatePoint = new Point(right + step, expression.calculate());

				expression = new Expression($"f({(right + step * 2).ToString().Replace(",", ".")})", function);
				_lastPoint = new Point(right + step * 2, expression.calculate());

				for (double i = left; i < right; i += step)
				{
					expression = new Expression($"f({i.ToString().Replace(",", ".")})", function);
					FunctionTable.Add(new Point(i, expression.calculate()));
				}
				Status = "Генерация прошла успешно!";
			}
			catch (Exception e)
			{
				Status = $"Опреация провалена. Причина: {e.Message}";
			}
		}
		private bool CanGenerateFunctionTableCommandxecute(object p)
		{
			return !(string.IsNullOrWhiteSpace(FunctionString) || string.IsNullOrWhiteSpace(LeftBoardText) || string.IsNullOrWhiteSpace(RightBoardText) || string.IsNullOrWhiteSpace(StepdText));
		}

		public ICommand CalculateDerivativeCommand { get; }
		private void OnCalculateDerivativeCommandExecuted(object p)
		{
			try
			{
				DerivativeTable.Clear();
				selectedDerivativeFunction();
				Status = "Рассчеты прошли успешно!";
			}
			catch (Exception e)
			{
				Status = $"Опреация провалена. Причина: {e.Message}";
			}
		}
		private bool CanCalculateDerivativeCommandxecute(object p)
		{
			bool ok = false;
			if (selectedDerivativeFunction.Method.Name == nameof(CalculateFirstDerivative))
				ok = !string.IsNullOrWhiteSpace(FunctionFirstDerivativeString);
			else if (selectedDerivativeFunction.Method.Name == nameof(CalculateSecondDerivative))
				ok = !string.IsNullOrWhiteSpace(FunctionSecondDerivativeString);
			else if (selectedDerivativeFunction.Method.Name == nameof(CalculateThirdDerivative))
				ok = !string.IsNullOrWhiteSpace(FunctionThirdDerivativeString);
			return ok && CanGenerateFunctionTableCommandxecute(p);
		}
		#endregion



		// Ахтунг, Внимание, В домашних условиях не повторять - говнокод

		private void CalculateFirstDerivative()
		{
			Function f = new Function(FunctionFirstDerivativeString);
			Expression expression;
			double doubleStep = Convert.ToDouble(StepdText) * 2;
			double firstCalculatedValue = (FunctionTable[1].Y - secondPoint.Y) / doubleStep;
			expression = new Expression($"f({FunctionTable[0].X.ToString().Replace(",", ".")})", f);
			DerivativeTable.Add(new DerivativeComparator(FunctionTable[0].X, firstCalculatedValue, expression.calculate()));
			double value;
			for (int i = 1; i < FunctionTable.Count - 1; i++)
			{
				value = (FunctionTable[i + 1].Y - FunctionTable[i - 1].Y) / doubleStep;
				expression = new Expression($"f({FunctionTable[i].X.ToString().Replace(",", ".")})", f);
				DerivativeTable.Add(new DerivativeComparator(FunctionTable[i].X, value, expression.calculate()));
			}
			double lastCalculatedValue = (_penultimatePoint.Y - FunctionTable[FunctionTable.Count - 2].Y) / doubleStep;
			expression = new Expression($"f({FunctionTable[FunctionTable.Count - 1].X.ToString().Replace(",", ".")})", f);
			DerivativeTable.Add(new DerivativeComparator(FunctionTable[FunctionTable.Count - 1].X, lastCalculatedValue, expression.calculate()));
		}

		private void CalculateSecondDerivative()
		{
			Function f = new Function(FunctionSecondDerivativeString);
			Expression expression;
			double step = Convert.ToDouble(StepdText);
			double quadroStep = step * step;
			double first = (secondPoint.Y - 2 * FunctionTable[0].Y + FunctionTable[1].Y) / quadroStep;
			expression = new Expression($"f({FunctionTable[0].X.ToString().Replace(",", ".")})", f);
			DerivativeTable.Add(new DerivativeComparator(FunctionTable[0].X, first, expression.calculate()));
			double value;
			for (int i = 1; i < FunctionTable.Count - 1; i++)
			{
				value = (FunctionTable[i - 1].Y - 2 * FunctionTable[i].Y + FunctionTable[i + 1].Y) / quadroStep;
				expression = new Expression($"f({FunctionTable[0].X.ToString().Replace(",", ".")})", f);
				DerivativeTable.Add(new DerivativeComparator(FunctionTable[i].X, value, expression.calculate()));
			}
			double last = (FunctionTable[FunctionTable.Count - 2].Y - 2 * FunctionTable[FunctionTable.Count - 1].Y + _penultimatePoint.Y) / quadroStep;
			expression = new Expression($"f({FunctionTable[FunctionTable.Count - 1].X.ToString().Replace(",", ".")})", f);
			DerivativeTable.Add(new DerivativeComparator(FunctionTable[FunctionTable.Count - 1].X, last, expression.calculate()));
		}

		private void CalculateThirdDerivative()
		{
			Function f = new Function(FunctionThirdDerivativeString);
			Expression expression;
			double step = Convert.ToDouble(StepdText);
			double tripleStep = step * step * step * 2;
			double firstValue = (-1 * _firstPoint.Y + 2 * secondPoint.Y - 2 * FunctionTable[1].Y + FunctionTable[2].Y) / tripleStep;
			expression = new Expression($"f({FunctionTable[0].X.ToString().Replace(",", ".")})", f);
			DerivativeTable.Add(new DerivativeComparator(FunctionTable[0].X, firstValue, expression.calculate()));
			double secondValue = (-1 * secondPoint.Y + 2 * FunctionTable[0].Y - 2 * FunctionTable[2].Y + FunctionTable[3].Y) / tripleStep;
			expression = new Expression($"f({FunctionTable[1].X.ToString().Replace(",", ".")})", f);
			DerivativeTable.Add(new DerivativeComparator(FunctionTable[1].X, secondValue, expression.calculate()));
			double value;
			for (int i = 2; i < FunctionTable.Count - 2; i++)
			{
				value = (-1 * FunctionTable[i - 2].Y + 2 * FunctionTable[i - 1].Y - 2 * FunctionTable[i + 1].Y + FunctionTable[i + 2].Y) / tripleStep;
				expression = new Expression($"f({FunctionTable[i].X.ToString().Replace(",", ".")})", f);
				DerivativeTable.Add(new DerivativeComparator(FunctionTable[i].X, value, expression.calculate()));
			}
			double penultimateValue = (-1 * FunctionTable[FunctionTable.Count - 4].Y + 2 * FunctionTable[FunctionTable.Count - 3].Y - 2 * FunctionTable[FunctionTable.Count - 1].Y + _penultimatePoint.Y) / tripleStep;
			expression = new Expression($"f({FunctionTable[FunctionTable.Count - 2].X.ToString().Replace(",", ".")})", f);
			DerivativeTable.Add(new DerivativeComparator(FunctionTable[FunctionTable.Count - 2].X, penultimateValue, expression.calculate()));
			double lastValue = (-1 * FunctionTable[FunctionTable.Count - 3].Y + 2 * FunctionTable[FunctionTable.Count - 2].Y - 2 * _penultimatePoint.Y + _lastPoint.Y) / tripleStep;
			expression = new Expression($"f({FunctionTable[FunctionTable.Count - 1].X})", f);
			DerivativeTable.Add(new DerivativeComparator(FunctionTable[FunctionTable.Count - 1].X, lastValue, expression.calculate()));
		}
	}
}
