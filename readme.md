# 📊 Desafio Técnico - Investimentos e Cotações B3

Este projeto foi desenvolvido como parte de um desafio técnico para uma vaga de desenvolvedor. O sistema simula a operação de uma corretora de investimentos, permitindo acompanhar operações de clientes, posições, lucro/prejuízo, cotações e estatísticas financeiras.

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core (MySQL)
- Dapper (consultas de alta performance)
- Kafka (Worker com consumo de cotações)
- Polly (Retry, Circuit Breaker e Fallback)
- Swagger / OpenAPI
- xUnit (testes unitários)
- AutoMapper
- Docker (para execução local opcional)
- Blazor Server (apresentação visual opcional)

## 🧩 Estrutura do Projeto

- **Domain**: Entidades, interfaces e modelos de domínio
- **Application**: Regras de negócio e serviços de aplicação
- **Infrastructure**:
  - `Data`: Repositórios, EF Core, Dapper
  - `Api`: Integração com cotação externa (B3 API)
  - `KafkaWorker`: Worker service com consumidor Kafka
- **Presentation**:
  - API REST (Swagger)
  - Blazor UI (opcional)
- **Tests**: Testes unitários com xUnit e Theory

## 📝 Funcionalidades Atendidas

✅ Cálculo do preço médio ponderado por ativo  
✅ Posição por papel (quantidade, preço médio e lucro/prejuízo)  
✅ Posição global do cliente  
✅ Total de corretagem por cliente  
✅ API para buscar última cotação de um ativo  
✅ API para consultar P&L, posições, top 10 clientes  
✅ Worker Kafka com retry, fallback e circuit breaker  
✅ Atualização em tempo real das posições com base na cotação  
✅ AutoMapper, logs e observabilidade com Polly  
✅ Regras de idempotência para evitar duplicações  
✅ Testes unitários com validações positivas e negativas  

## 📡 API Endpoints

- `GET /api/clientes/{usuarioId}/posicao`: Posição global do cliente
- `GET /api/clientes/{usuarioId}/ativos/{ticker}/preco-medio`: Preço médio de um ativo
- `GET /api/ativos/{ticker}/cotacao`: Última cotação do ativo
- `GET /api/admin/top-clientes-posicao`: Top 10 clientes por valor de posição
- `GET /api/admin/top-clientes-corretagem`: Top 10 clientes que mais pagaram corretagem
- `GET /api/admin/corretagem-total`: Valor total ganho pela corretora

Documentação completa disponível em `/swagger`.

## ⚙️ Execução Local

1. **Banco de Dados MySQL**: configure no `appsettings.json`  
2. **Kafka**: pode ser executado via Docker ou ambiente local  
3. **Executar API**:  
   ```bash
   dotnet run --project TesteTecnicoItau.Presentation

