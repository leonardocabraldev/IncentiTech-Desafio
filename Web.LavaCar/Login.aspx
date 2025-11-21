<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.LavaCar.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>LavaCar - Login</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="~/Content/Global.css" rel="stylesheet" />
</head>
<body>
    <div class="wrapper-center">
        <form id="form1" runat="server" class="w-100" style="display: flex; justify-content: center;">
        <div class="login-wrapper">
            <i class="bi bi-droplet-half login-icon"></i>
            <h2>LavaCar Login</h2>

            <div class="mb-3 text-start form-outline">       
                <label for="txtUsername" class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control form-control-lg" CausesValidation="true" />
                <asp:CustomValidator ID="cvUsername" runat="server" ControlToValidate="txtUsername"
                    CssClass="error-message"
                    Display="Dynamic"
                    ValidationGroup="Login"
                    ValidateEmptyText="true"
                    ErrorMessage="Username inválido!"
                    OnServerValidate="cvUsername_ServerValidate">
                </asp:CustomValidator>
            </div>

            <div class="mb-3 text-start form-outline">  
                <label for="txtPassword" class="form-label">Senha</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control form-control-lg" CausesValidation="true"  />
                <asp:CustomValidator ID="cvPassword" runat="server" ControlToValidate="txtPassword"
                    CssClass="error-message"
                    Display="Dynamic"
                    ValidationGroup="Login"
                    ValidateEmptyText="true"
                    ErrorMessage="Senha inválida! (mínimo 6 caracteres)"
                    OnServerValidate="cvPassword_ServerValidate">
                </asp:CustomValidator>
            </div>

            <asp:Button ID="btLogar" runat="server" Text="Entrar" CssClass="btn btn-login" OnClick="btLogar_Click" CausesValidation="true" ValidationGroup="Login"/>

            <div class="footer-text">
                <span>© 2025 LavaCar System</span>
            </div>
        </div>
    </form>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
