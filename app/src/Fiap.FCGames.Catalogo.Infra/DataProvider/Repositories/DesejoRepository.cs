using Amazon.DynamoDBv2.DataModel;
using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateDesejo;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Dynamo;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories;

public class DesejoRepository : IDesejoRepository
{
    private readonly IDynamoDBContext _dynamoContext;

    public DesejoRepository(IDynamoDBContext dynamoContext) => _dynamoContext = dynamoContext;

    public async Task<List<Desejo>> ListarPorUsuarioAsync(Guid usuarioId)
    {
        var documentos = await _dynamoContext.QueryAsync<DesejoDocument>(usuarioId.ToString()).GetRemainingAsync();
        return documentos.Select(ParaDominio).ToList();
    }

    public async Task<Desejo?> ObterAsync(Guid usuarioId, Guid jogoId)
    {
        var documento = await _dynamoContext.LoadAsync<DesejoDocument>(usuarioId.ToString(), jogoId.ToString());
        return documento is null ? null : ParaDominio(documento);
    }

    public Task AdicionarAsync(Desejo desejo) => _dynamoContext.SaveAsync(ParaDocumento(desejo));

    public Task RemoverAsync(Guid usuarioId, Guid jogoId)
        => _dynamoContext.DeleteAsync<DesejoDocument>(usuarioId.ToString(), jogoId.ToString());

    private static Desejo ParaDominio(DesejoDocument documento) => new()
    {
        UsuarioId = Guid.Parse(documento.UsuarioId),
        JogoId = Guid.Parse(documento.JogoId),
        AdicionadoEm = documento.AdicionadoEm
    };

    private static DesejoDocument ParaDocumento(Desejo desejo) => new()
    {
        UsuarioId = desejo.UsuarioId.ToString(),
        JogoId = desejo.JogoId.ToString(),
        AdicionadoEm = desejo.AdicionadoEm
    };
}
