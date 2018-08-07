namespace SSH.Core.DTO
{
    public interface ILoggableDTO<TEntity>
    {
        void ConvertFromLogEntity(TEntity entity);
    }
}
