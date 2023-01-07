namespace Domain.Interfaces
{
    public interface IDbConverter<TDbto, TEntity>
    {
        TEntity FromDbDto(TDbto dto);
        TDbto ToDbDto(TEntity entity);
    }
}