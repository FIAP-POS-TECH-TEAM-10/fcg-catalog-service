using Amazon.DynamoDBv2.DataModel;
using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Dynamo;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories;

public class JogoRepository : IJogoRepository
{
    private readonly IDynamoDBContext _dynamoContext;

    public JogoRepository(IDynamoDBContext dynamoContext) => _dynamoContext = dynamoContext;

    public async Task<List<Jogo>> ListarTodosAsync()
    {
        var documentos = await _dynamoContext.ScanAsync<JogoDocument>([]).GetRemainingAsync();
        return documentos.Select(ParaDominio).ToList();
    }

    public async Task<Jogo?> ObterPorIdAsync(Guid id)
    {
        var documento = await _dynamoContext.LoadAsync<JogoDocument>(id.ToString());
        return documento is null ? null : ParaDominio(documento);
    }

    public Task AdicionarAsync(Jogo jogo) => _dynamoContext.SaveAsync(ParaDocumento(jogo));

    public Task AtualizarAsync(Jogo jogo) => _dynamoContext.SaveAsync(ParaDocumento(jogo));

    public Task RemoverAsync(Jogo jogo) => _dynamoContext.DeleteAsync<JogoDocument>(jogo.Id.ToString());

    private static Jogo ParaDominio(JogoDocument documento) => new()
    {
        Id = Guid.Parse(documento.Id),
        Nome = documento.Nome,
        Descricao = documento.Descricao,
        Preco = documento.Preco,
        DataCadastro = documento.DataCadastro
    };

    private static JogoDocument ParaDocumento(Jogo jogo) => new()
    {
        Id = jogo.Id.ToString(),
        Nome = jogo.Nome,
        Descricao = jogo.Descricao,
        Preco = jogo.Preco,
        DataCadastro = jogo.DataCadastro
    };
}
