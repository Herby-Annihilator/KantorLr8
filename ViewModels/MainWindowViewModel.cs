using KantorLr8.Infrastructure.Commands;
using KantorLr8.Model.Data;
using KantorLr8.ViewModels.Base;
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

		public MainWindowViewModel()
		{
			selectedDerivativeFunction = CalculateFirstDerivative;
			SelectCalculatingDeriavtiveCommand = new LambdaCommand(OnSelectCalculatingDeriavtiveCommandExecuted, CanSelectCalculatingDeriavtiveCommandExecute);
			RemoveSelectedPointInFunctionTableCommand = new LambdaCommand(OnRemoveSelectedPointInFunctionTableCommandExecuted, CanRemoveSelectedPointInFunctionTableCommandExecute);
			AddNewPointInFunctionTableCommand = new LambdaCommand(OnAddNewPointInFunctionTableCommandExecuted, CanAddNewPointInFunctionTableCommandxecute);
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
		#endregion



		// Ахтунг, Внимание, В домашних условиях не повторять - говнокод

		private void CalculateFirstDerivative()
		{

		}

		private void CalculateSecondDerivative()
		{

		}

		private void CalculateThirdDerivative()
		{

		}
	}
}
