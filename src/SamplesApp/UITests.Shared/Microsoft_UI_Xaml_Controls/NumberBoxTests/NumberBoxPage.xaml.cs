﻿#pragma warning disable CS0105 // duplicate namespace because of WinUI source conversion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Uno.UI.Samples.Controls;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;


namespace UITests.Shared.Microsoft_UI_Xaml_Controls.NumberBoxTests
{
	[Sample("MUX", Name= "NumberBox")]
	public sealed partial class NumberBoxPage : UserControl
	{
		public DataModelWithINPC DataModelWithINPC { get; set; } = new DataModelWithINPC();

		public NumberBoxPage()
		{
			this.InitializeComponent();

			TestNumberBox.RegisterPropertyChangedCallback(NumberBox.TextProperty, new DependencyPropertyChangedCallback(TextPropertyChanged));
		}

		private void InputScope_Changed(object sender, RoutedEventArgs e)
		{
			if (TestNumberBox != null &&
				sender is ComboBox comboBox &&
				comboBox.SelectedItem is ComboBoxItem item)
			{
				var scopeName = new InputScopeName();
				scopeName.NameValue = (InputScopeNameValue)Enum.Parse(typeof(InputScopeNameValue), item.Content?.ToString() ?? string.Empty, true);

				var scope = new InputScope();
				scope.Names.Add(scopeName);

				TestNumberBox.InputScope = scope;

				// Help testing by returning focus to the NumberBox to see the keyboard change
				TestNumberBox.Focus(FocusState.Keyboard);
			}
		}

		private void SpinMode_Changed(object sender, RoutedEventArgs e)
		{
			if (TestNumberBox != null)
			{
				if (SpinModeComboBox.SelectedIndex == 0)
				{
					TestNumberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Hidden;
				}
				else if (SpinModeComboBox.SelectedIndex == 1)
				{
					TestNumberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact;
				}
				else if (SpinModeComboBox.SelectedIndex == 2)
				{
					TestNumberBox.SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline;
				}
			}
		}

		private void TextAlignment_Changed(object sender, RoutedEventArgs e)
		{
			if (TestNumberBox != null)
			{
				if (TextAlignmentComboBox.SelectedIndex == 0)
				{
					TestNumberBox.TextAlignment = Windows.UI.Xaml.TextAlignment.Left;
				}
				else if (TextAlignmentComboBox.SelectedIndex == 1)
				{
					TestNumberBox.TextAlignment = Windows.UI.Xaml.TextAlignment.Center;
				}
				else if (TextAlignmentComboBox.SelectedIndex == 2)
				{
					TestNumberBox.TextAlignment = Windows.UI.Xaml.TextAlignment.Right;
				}
			}
		}

		private void Validation_Changed(object sender, RoutedEventArgs e)
		{
			if (TestNumberBox != null)
			{
				if (ValidationComboBox.SelectedIndex == 0)
				{
					TestNumberBox.ValidationMode = NumberBoxValidationMode.InvalidInputOverwritten;
				}
				else if (ValidationComboBox.SelectedIndex == 1)
				{
					TestNumberBox.ValidationMode = NumberBoxValidationMode.Disabled;
				}
			}
		}

		private void MinCheckBox_CheckChanged(object sender, RoutedEventArgs e)
		{
			MinNumberBox.IsEnabled = (bool)MinCheckBox.IsChecked;
			MinValueChanged(null, null);
		}

		private void MaxCheckBox_CheckChanged(object sender, RoutedEventArgs e)
		{
			MaxNumberBox.IsEnabled = (bool)MaxCheckBox.IsChecked;
			MaxValueChanged(null, null);
		}

		private void MinValueChanged(object sender, object e)
		{
			if (TestNumberBox != null)
			{
				TestNumberBox.Minimum = (bool)MinCheckBox.IsChecked ? MinNumberBox.Value : double.MinValue;
			}
		}

		private void MaxValueChanged(object sender, object e)
		{
			if (TestNumberBox != null)
			{
				TestNumberBox.Maximum = (bool)MaxCheckBox.IsChecked ? MaxNumberBox.Value : double.MaxValue;
			}
		}

		private void NumberBoxValueChanged(object sender, NumberBoxValueChangedEventArgs e)
		{
			if (TestNumberBox != null && NewValueTextBox != null && OldValueTextBox != null)
			{
				NewValueTextBox.Text = e.NewValue.ToString();
				OldValueTextBox.Text = e.OldValue.ToString();
			}
		}

		private void CustomFormatterCheckBox_CheckChanged(object sender, RoutedEventArgs e)
		{
			if (CustomFormatterCheckBox.IsChecked.GetValueOrDefault())
			{
				DecimalFormatter formatter = new DecimalFormatter();
				formatter.IntegerDigits = 1;
				formatter.FractionDigits = 2;
				formatter.NumeralSystem = "ArabExt";
				formatter.NumberRounder = new IncrementNumberRounder { Increment = 0.25 };

				TestNumberBox.NumberFormatter = formatter;
			}
			else
			{
				TestNumberBox.NumberFormatter = null;
			}
		}

		private void SetTextButton_Click(object sender, RoutedEventArgs e)
		{
			TestNumberBox.Text = "15";
		}

		private void SetValueButton_Click(object sender, RoutedEventArgs e)
		{
			TestNumberBox.Value = 42;
		}

		private void SetNaNButton_Click(object sender, RoutedEventArgs e)
		{
			TestNumberBox.Value = Double.NaN;
		}

		private void SetTwoWayBoundNaNButton_Click(object sender, RoutedEventArgs e)
		{
			DataModelWithINPC.Value = Double.NaN;
			TwoWayBoundNumberBoxValue.Text = TwoWayBoundNumberBox.Value.ToString();
		}

		private void ToggleHeaderValueButton_Click(object sender, RoutedEventArgs e)
		{
			if (HeaderTestingNumberBox.Header is null)
			{
				var demoHeader = new TextBlock();
				demoHeader.SetValue(AutomationProperties.NameProperty, "NumberBoxHeaderClippingDemoHeader");
				demoHeader.Text = "Test header";
				HeaderTestingNumberBox.Header = demoHeader;
			}
			else
			{
				// Switching between normal header and empty string header
				if (HeaderTestingNumberBox.Header as string is null)
				{
					HeaderTestingNumberBox.Header = "";
				}
				else
				{
					HeaderTestingNumberBox.Header = null;
				}
			}
		}

		private void ToggleHeaderTemplateValueButton_Click(object sender, RoutedEventArgs e)
		{
			if (HeaderTemplateTestingNumberBox.HeaderTemplate is null)
			{
				string templateString =
				@"<DataTemplate 
                xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""> 
                    <TextBlock AutomationProperties.Name=""HeaderTemplateTestingBlock"" Text=""Some text""/> 
                </DataTemplate>";
				HeaderTemplateTestingNumberBox.HeaderTemplate = XamlReader.Load(templateString) as DataTemplate;
			}
			else
			{
				// Switching between normal header and empty string header
				HeaderTemplateTestingNumberBox.HeaderTemplate = null;
			}
		}

		private void TextPropertyChanged(DependencyObject o, DependencyProperty p)
		{
			TextTextBox.Text = TestNumberBox.Text;
		}

		private void ScrollviewerWithScroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			VerticalOffsetDisplayBlock.Text = (sender as Windows.UI.Xaml.Controls.ScrollViewer).VerticalOffset.ToString();
		}
	}

	public class DataModelWithINPC : INotifyPropertyChanged
	{
		private double _value;

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public double Value
		{
			get => _value;
			set
			{
				if (value != _value)
				{
					_value = value;
					OnPropertyChanged(nameof(this.Value));
				}
			}
		}

	}
}
