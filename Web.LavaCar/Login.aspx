<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.LavaCar.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>LavaCar - Login</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />

    <style>
        body {
            height: 100vh;
            margin: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            background: linear-gradient(135deg, #e0f7fa, #bbdefb);
            font-family: 'Inter', 'Segoe UI', sans-serif;
        }

        .login-wrapper {
            backdrop-filter: blur(12px);
            background: rgba(255, 255, 255, 0.3);
            border: 1px solid rgba(255, 255, 255, 0.4);
            border-radius: 18px;
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
            width: 100%;
            max-width: 380px;
            padding: 40px 35px;
            text-align: center;
        }

        .login-icon {
            font-size: 3rem;
            color: #1976d2;
            margin-bottom: 10px;
        }

        h2 {
            font-weight: 600;
            color: #0d47a1;
            margin-bottom: 1.5rem;
        }

        .form-control {
            background-color: rgba(255, 255, 255, 0.6);
            border: none;
            border-radius: 10px;
            padding: 12px;
            font-size: 1rem;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.1);
            transition: all 0.2s ease;
        }

        .form-control:focus {
            box-shadow: 0 0 0 3px rgba(25, 118, 210, 0.2);
            background-color: white;
        }

        .btn-login {
            background-color: #1976d2;
            border: none;
            border-radius: 12px;
            padding: 12px;
            font-size: 1rem;
            font-weight: 500;
            color: white;
            width: 100%;
            transition: 0.3s;
        }

        .btn-login:hover {
            background-color: #0d47a1;
        }

        .error-message {
            color: #d32f2f;
            font-size: 0.9rem;
            display: block;
            margin-top: 4px;
        }

        .footer-text {
            margin-top: 1.5rem;
            font-size: 0.85rem;
            color: #555;
        }

        .form-label {
            display: none; /* minimalista */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="w-100" style="display: flex; justify-content: center;">
        <div class="login-wrapper">
            <i class="bi bi-droplet-half login-icon"></i>
            <h2>LavaCar Login</h2>

            <div class="mb-3 text-start">
                <label for="txtEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" />
                <asp:CustomValidator ID="cvEmail" runat="server" ControlToValidate="txtEmail"
                    CssClass="error-message"
                    ErrorMessage="Email inválido!"
                    OnServerValidate="CustomValidator1_ServerValidate">
                </asp:CustomValidator>
            </div>

            <div class="mb-3 text-start">
                <label for="txtSenha" class="form-label">Senha</label>
                <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="form-control" placeholder="Senha" />
            </div>

            <asp:Button ID="btLogar" runat="server" Text="Entrar" CssClass="btn btn-login" OnClick="Button1_Click" />

            <div class="footer-text">
                <span>© 2025 LavaCar System</span>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
