# 📝 Gerenciador de Tarefas - API RESTful (.NET 8)

Este projeto é uma aplicação de gerenciamento de tarefas, desenvolvida como parte de um desafio técnico. A arquitetura foi projetada com foco em princípios SOLID, Clean Architecture e testabilidade.

---

## 🚀 Tecnologias

- [.NET 8](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- Entity Framework Core (com SQL Server e InMemory)
- xUnit + Moq (testes unitários)
- Swagger (OpenAPI)
- Clean Architecture + DDD

---

## 📦 Estrutura do Projeto

```
├── Api                    # Camada de apresentação (Controllers)
├── Application            # Camada de aplicação (DTOs, Services, Interfaces)
├── Domain                 # Camada de domínio (Entidades, Enums, Contratos)
├── Infrastructure         # Camada de persistência (Data, Repositories)
├── Tests                  # Testes unitários
└── README.md              # Documentação do projeto
```

---

## 📌 Funcionalidades

- ✅ Criar tarefa
- ✅ Obter tarefa por ID
- ✅ Listar tarefas com filtros por status e vencimento
- ✅ Atualizar tarefa
- ✅ Remover tarefa
- ✅ Enum legível no Swagger (`StatusTarefa`)
- ✅ Testes unitários com cobertura de serviços e controllers

---

## 🔧 Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/seuusuario/seurepo.git
   cd seurepo
   ```

2. Restaure os pacotes:
   ```bash
   dotnet restore
   ```

3. Execute a aplicação:
   ```bash
   dotnet run --project Api
   ```

4. Acesse no navegador:
   ```
   http://localhost:5000/swagger
   ```

---

## 📂 Endpoints

| Método | Rota                 | Ação                            |
|--------|----------------------|---------------------------------|
| GET    | `/api/tarefa`        | Lista tarefas com filtros       |
| GET    | `/api/tarefa/{id}`   | Obtém uma tarefa por ID         |
| POST   | `/api/tarefa`        | Cria uma nova tarefa            |
| PUT    | `/api/tarefa/{id}`   | Atualiza uma tarefa existente   |
| DELETE | `/api/tarefa/{id}`   | Remove uma tarefa               |

---

## 🧪 Executar Testes

```bash
dotnet test
```

Os testes cobrem:

- `TarefaRepository`
- `TarefaService`
- `TarefaController`
- Comportamentos para cenários de sucesso e falha

---

## 📘 StatusTarefa (Enum)

| Valor | Descrição    |
|-------|--------------|
| 0     | Pendente     |
| 1     | Em Andamento |
| 2     | Concluído    |

Exibido corretamente no Swagger via `SwaggerSchemaFilter`.

---

## ✍️ Autor

**Paulo Trinchão**  
Arquiteto de Software , especialista em desenvimento de soluções WEB, ambiente On primese e Cloud(Azure e AWS).  

[LinkedIn](https://www.linkedin.com/in/paulo-trinchao) · [GitHub](https://github.com/paulotrinchao)
