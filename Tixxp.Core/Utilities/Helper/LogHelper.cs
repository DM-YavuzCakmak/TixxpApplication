using System.Text.Json;
using Tixxp.Core.Utilities.Results.Concrete;
namespace Tixxp.Core.Utilities.Helper;

public static class LogHelper
{
    public static CompareDataResult CompareData(HistoryLogModel historyLogModel)
    {
        var result = new CompareDataResult { IsSuccess = true };

        try
        {
            if (string.IsNullOrWhiteSpace(historyLogModel.OldData) || string.IsNullOrWhiteSpace(historyLogModel.NewData))
            {
                result.IsSuccess = false;
                result.Message = "OldData or NewData is null or empty.";
                return result;
            }

            var oldDataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(historyLogModel.OldData);
            var newDataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(historyLogModel.NewData);

            if (oldDataDict == null || newDataDict == null)
            {
                result.IsSuccess = false;
                result.Message = "Deserialization failed for OldData or NewData.";
                return result;
            }

            foreach (var property in oldDataDict)
            {
                var columnName = property.Key;
                var oldValue = property.Value?.ToString();
                var newValue = newDataDict.ContainsKey(columnName) ? newDataDict[columnName]?.ToString() : null;

                if (oldValue != newValue)
                {
                    historyLogModel.ColumnChanges.Add(new ColumnChange
                    {
                        ColumnName = columnName,
                        OldValue = oldValue,
                        NewValue = newValue
                    });
                }
            }

            result.ChangedColumns = historyLogModel.ColumnChanges;
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = $"Error in CompareData: {ex.Message}";
        }

        return result;
    }
}
