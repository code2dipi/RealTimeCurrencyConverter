namespace ConverterService.Service
{
    public interface IHttpHelpers
    {
        Task<TReturn?> Get<TReturn>(string url)
            where TReturn : class;
    }
}
