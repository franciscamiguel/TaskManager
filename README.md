# Task Manager
## Visão Geral

O Task Manager é uma aplicação para gerenciamento de tarefas com a capacidade de anexar arquivos e armazená-los no S3 da AWS. O sistema permite a criação, edição, visualização e exclusão de tarefas e seus respectivos anexos.

A aplicação é composta por um backend em ASP.NET Core que expõe uma API RESTful e um frontend em Angular para interação com o usuário.

## Pré Requisitos
- .NET 8
- NodeJs
- Angular CLI (Versão 18)
- Visual Studio ou VS Code
- Docker Desktop
- Conta no Localstack e acessar o icone do S3: https://app.localstack.cloud/inst/default/resources/s3

## Funcionalidades

### Backend
- **Gerenciamento de Tarefas:** API para adicionar, listar, editar e excluir tarefas.
- **Anexos de Arquivos:** Integração com o AWS S3 para upload, download e exclusão de arquivos anexados às tarefas.
- **Validação:** Implementação de validações automáticas no backend com notificações para o cliente.

### Frontend
- **Criação/edição de tarefas:** Interface para criação de novas tarefas ou edição de tarefas existentes.
- **Visualização de tarefas:** Listagem das tarefas com status de conclusão e links para visualização de anexos.
- **Exclusão de tarefas:** Exclusão de uma tarefa e seu anexo correspondente do S3

## Estrutura de Diretórios

### Backend - TaskManager.Api
- Controllers:
  - **TasksController:** Controlador responsável por gerenciar as operações de CRUD de tarefas.
  - **FilesController:** Controlador responsável por upload, download e exclusão de arquivos no S3.
- Models:
  - **Tasks:** Entidade principal representando uma tarefa.
  - **Dtos:** Contém os DTOs para comunicação de dados entre o cliente e o servidor.
- Services:
  - **TaskService:** Lógica de negócios para gerenciar as tarefas.
  - **StorageService:** Serviço para gerenciar o upload e exclusão de arquivos no AWS S3.
- **Notifier:** Sistema de notificações e gestão de erros para validações e respostas de erro.

### Frontend - TaskManager.UI
- components:
  - tasks: Componente principal de tarefas com listagem, criação e edição.
  - shared: Contém componentes reutilizáveis (como títulos e botões).
- services:
  - task.service.ts: Serviço que comunica o frontend com a API para gerenciar tarefas.
  - file.service.ts: Serviço para upload, download e exclusão de arquivos.

## Endpoints da API
### Tasks
- **GET /api/v1/tasks:** Lista todas as tarefas com paginação.
- **GET /api/v1/tasks/{id}:** Retorna uma tarefa específica.
- **POST /api/v1/tasks:** Cria uma nova tarefa.
- **PUT /api/v1/tasks/{id}:** Atualiza uma tarefa existente.
- **DELETE /api/v1/tasks/{id}:** Exclui uma tarefa (e seu anexo, se existir).

### Files
- **POST /api/v1/files/upload/{entityId}:** Faz o upload de um arquivo anexado a uma tarefa.
- **GET /api/v1/files/download?key={key}:** Faz o download de um arquivo armazenado no S3.
- **DELETE /api/v1/files/delete/{key}:** Exclui um arquivo do S3.
- **DELETE /api/v1/files/delete-multiple:** Exclui múltiplos arquivos do S3.

## Instalação

### Backend

1. Clone o repositório.
2. Navegue até o diretório src/TaskManager.Api
3. Configure a conexão com o banco de dados SQL Server no arquivo appsettings.json ou appsettings.Development.json
4. Configure as credenciais do AWS S3 no arquivo appsettings.json ou appsettings.Development.json.
5. Para uso local, não precisa modificar as credenciais do AWS S3, pois usará o LocalStack
6. Execute o comando ou clique no botão de execução (F5): ```dotnet run```
7. Ao executar a Api, o banco e as tabelas serão criadas automáticamente
8. Navegue até a pasta "Scripts" e execute todos para criação das stored procedures.

### Externo
Para ser possível utilizar o S3, deve-se fazer uso do LocalStack, uma ferramenta que simula serviços da Amazon localmente
1. Navegue até o diretório localstack
2. Abra um novo terminal e execute: ```docker-compose -f localstack/docker-compose.yml``` 

### Frontend
1. Navegue até o diretório src/TaskManager.UI
2. Certifique-se de que o nodejs e o angular cli já estejam instalados na sua máquina
3. Execute o comando: ```npm i```
4. Para rodar o frontend: ```ng s -o```

## Tecnologias Utilizadas

### Backend
- **ASP.NET Core:** Framework para construir a API RESTful.
- **Entity Framework Core:** ORM para interagir com o banco de dados.
- **AWS S3:** Armazenamento de arquivos.
- **LocalStack:** Serviço para simular o S3 localmente
- **FluentValidation:** Validação de dados de entrada.

### Frontend
- **Angular:** Framework para desenvolvimento de SPA (Single Page Application).
- **Bootstrap 5.3:** Framework CSS para design responsivo.
- **Ngx-Toastr:** Biblioteca para exibir notificações toast.

## Funcionalidades Adicionais
- **Remoção Segura de Arquivos:** Quando uma tarefa é excluída, o sistema garante que o anexo correspondente seja excluído do S3.
- **Upload de Arquivos:** Suporte a upload de arquivos no formato `multipart/form-data`, com integração ao S3.


## Considerações Finais
Este projeto é uma aplicação simples para gerenciar tarefas, com suporte a anexos armazenados na AWS S3. Ele serve como um exemplo prático de integração entre um backend construído com ASP.NET Core e um frontend feito com Angular.