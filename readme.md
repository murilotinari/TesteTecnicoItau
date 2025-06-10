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


# 📊 Estrutura e Justificativa do Banco de Dados

## 🗃️ Script SQL para Criação das Tabelas

```sql
CREATE TABLE usuarios (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL,
    pct_corretagem DECIMAL(5,2) NOT NULL
);

CREATE TABLE ativos (
    id INT PRIMARY KEY AUTO_INCREMENT,
    codigo VARCHAR(10) NOT NULL,
    nome VARCHAR(100) NOT NULL
);

CREATE TABLE operacoes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    usuario_id INT NOT NULL,
    ativo_id INT NOT NULL,
    qtd INT NOT NULL,
    preco_unit DECIMAL(18,6) NOT NULL,
    tipo_op VARCHAR(10) NOT NULL,
    corretagem DECIMAL(10,2) NOT NULL,
    data_hora DATETIME NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (ativo_id) REFERENCES ativos(id),
    INDEX idx_usuario_ativo_data (usuario_id, ativo_id, data_hora)
);

CREATE TABLE cotacoes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    ativo_id INT NOT NULL,
    preco_unit DECIMAL(18,6) NOT NULL,
    data_hora DATETIME NOT NULL,
    FOREIGN KEY (ativo_id) REFERENCES ativos(id),
    INDEX idx_ativo_data (ativo_id, data_hora)
);

CREATE TABLE posicoes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    usuario_id INT NOT NULL,
    ativo_id INT NOT NULL,
    qtd INT NOT NULL,
    preco_medio DECIMAL(18,6) NOT NULL,
    pl DECIMAL(18,6) NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (ativo_id) REFERENCES ativos(id),
    UNIQUE KEY uq_usuario_ativo (usuario_id, ativo_id)
);
```

## 📚 Justificativa da Escolha dos Tipos de Dados

- **`INT`**: Usado em chaves primárias, estrangeiras e campos de quantidade. É leve, performático e suficiente para o volume esperado.
- **`VARCHAR`**: Ideal para campos textuais como nomes, e-mails e códigos de ativos por sua flexibilidade e compatibilidade com índices.
- **`DECIMAL(18,6)`**: Escolhido para garantir alta precisão em cálculos financeiros, como preços e lucro/prejuízo (P&L), evitando problemas de arredondamento.
- **`DATETIME`**: Fundamental para registrar e consultar eventos com precisão temporal, como operações e cotações.

## 🚀 Índices Propostos e Justificativas

### 🎯 Índice para `operacoes`:

```sql
CREATE INDEX idx_usuario_ativo_data ON operacoes (usuario_id, ativo_id, data_hora);
```

**Justificativa**:  
Esse índice acelera as consultas que filtram operações de um usuário para um ativo específico nos últimos 30 dias. Ele também facilita ordenações por data de operação, essenciais para cálculos como preço médio e P&L.


## 🧪 Consulta SQL Otimizada

```sql
SELECT *
FROM operacoes
WHERE usuario_id = @usuario_id
  AND ativo_id = @ativo_id
  AND data_hora >= NOW() - INTERVAL 30 DAY
ORDER BY data_hora DESC;
```

**Objetivo**:  
Essa query retorna todas as operações feitas por um usuário para um determinado ativo no intervalo de 30 dias, ordenadas da mais recente para a mais antiga.


## 🧠 Observações Finais

- As tabelas seguem a convenção `snake_case`, comum em bancos relacionais.
- As relações entre entidades são mantidas com `FOREIGN KEY` para garantir **integridade referencial**.
- A tabela `posicoes` é atualizada automaticamente em tempo de execução a partir de eventos de cotação recebidos via **Kafka (Worker)**.
