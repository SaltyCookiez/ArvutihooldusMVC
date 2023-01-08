using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArvutihooldusMVC
{
    public enum ComputerType
    {
        Laptop,
        Desktop
    }

    public enum Service
    {
        Cleaning,
        Repairing,
        Assembly,
        Antivirus,
        Updating
    }

    public static class SelectLists
    {
        public static SelectList GetComputerTypeSelectList()
        {
            var enumValues = Enum.GetValues(typeof(ComputerType)).Cast<ComputerType>().Select(e => new { Value = e.ToString(), Text = e.ToString() }).ToList();

            return new SelectList(enumValues, "Value", "Text", "");
        }

        public static SelectList GetServiceSelectList()
        {
            var enumValues = Enum.GetValues(typeof(Service)).Cast<Service>().Select(e => new { Value = e.ToString(), Text = e.ToString() }).ToList();

            return new SelectList(enumValues, "Value", "Text", "");
        }
    }
}
