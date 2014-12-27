using System;

namespace VKAUDIO
{
    public class SearchingErrorArgs:EventArgs
    {
        public readonly string Message = "Ошибка при поиске, возможно отсутствует соединение с сервером";
    }
}