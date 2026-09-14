namespace Fiap.FCGames.Catalogo.Application.Queries.Desejos.ListarDesejos;

public record DesejoResponse(Guid JogoId, string NomeJogo, decimal Preco, DateTime AdicionadoEm);
