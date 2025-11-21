# Projeto de Agendamento e Serviços LavaCar

Gerencia serviços e agendamentos com validações de limite de atendimentos e controle de serviços ativos. Inclui interface web com modais, paginação e validação server-side.

---

## Banco de dados

Todos os scripts do banco de dados estão dentro da pasta `Infrastructure/SQL`.

1. **Criar banco e tabelas**
   - Abra o SQL Server Management Studio (SSMS) ou outro cliente SQL.
   - Navegue até a pasta `Infrastructure/SQL`.
   - Execute o script `CreateDatabase.sql` para criar o banco e as tabelas.

2. **Popular dados de teste (opcional)**
   - Execute o script `Create Database.sql` para inserir serviços e usuários de teste.

3. **Verificar conexão**
   - Certifique-se de que a string de conexão no `Web.config` aponta para o banco correto.

## Como rodar o projeto

1. **Clonar repositório**
   - `git clone <URL_DO_REPOSITORIO>`
   - `cd <NOME_DO_PROJETO>`

2. **Abrir no Visual Studio**
   - Abrir a solução `.sln`.
   - Garantir que o .NET Framework usado esteja instalado (4.7.2).

3. **Configurar conexão com o banco**
   - Editar `Web.config` na seção `<connectionStrings>` com os dados do seu SQL Server.

4. **Restaurar pacotes NuGet**
   - Clique com o botão direito na solução → `Restore NuGet Packages`.

5. **Executar o projeto**
   - Pressione `F5` ou `Start Debugging`.
  



