using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Converters
{
    public class AmountToPercentageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value as BudgetDTO != null)
            {
                var selectedBudget = value as BudgetDTO;
                if (selectedBudget == null || selectedBudget.TotalSpentAmount <= 0)
                {
                    return 0;
                }
                if (selectedBudget.TotalSpentAmount > selectedBudget.Amount) return 100; 
                return (selectedBudget.TotalSpentAmount * 100 / selectedBudget.Amount);
            }
            var selectedGoal = value as GoalDTO;
            if (selectedGoal == null || selectedGoal.TotalSavedAmount <= 0)
            {
                return 0;
            }
            if (selectedGoal.TotalSavedAmount > selectedGoal.Amount) return 100;
            return (selectedGoal.TotalSavedAmount * 100 / selectedGoal.Amount);
        }
        //public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        //{
        //    if (value == null || (int)value == 0)
        //    {
        //        return (double)0;
        //    }
        //    if (parameter == null) return 0;
        //    return ((int)value /(double)parameter) * 100;
        //}

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
