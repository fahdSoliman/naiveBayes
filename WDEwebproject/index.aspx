<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WDEwebproject.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Heart Disease Prediction</title>
    <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous" />
    <meta name="description" content="Naive Bayes Algorithm for Heart Disease Prediction" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
    <nav class="navbar navbar-light bg-light">
        <span class="navbar-brand mb-2 h1">Naive Bayes Algorithm for Heart Disease Prediction</span>
    </nav>
    <div class="container">
        <nav>
            <div class="nav nav-tabs" id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav-home-tab" data-toggle="tab" href="#nav-predict" role="tab" aria-controls="nav-predict" aria-selected="true">Predict For Disease</a>
                <a class="nav-item nav-link" id="nav-profile-tab" data-toggle="tab" href="#nav-explain" role="tab" aria-controls="nav-explain" aria-selected="false">Explaining of algorithm</a>
                <a class="nav-item nav-link" id="nav-contact-tab" data-toggle="tab" href="#nav-contact" role="tab" aria-controls="nav-contact" aria-selected="false">References</a>
            </div>
        </nav>
    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-predict" role="tabpanel" aria-labelledby="nav-home-tab">
            <div class="container">
                <form id="form1" runat="server">
                    <div class="row">

                        <%--First form for predicting Using Programmed Algorithm in C#--%>
                        <div class="col-md-6">
                            <br />
                            <h5>
                                Using Programmed Algorithm in C#
                            </h5>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Age</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="age" runat="server" CssClass="form-control" placeholder="age"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ControlToValidate="age" ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">chest pain type</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList CssClass="form-control" ID="chest_pain_type" runat="server" DataSourceID="chestpainDS" DataTextField="chest_pain_type" 
                                        DataValueField="chest_pain_type"></asp:DropDownList>
                                    <asp:SqlDataSource ID="chestpainDS" runat="server" ConnectionString="<%$ ConnectionStrings:heartDiseaseConnectionString %>" 
                                        SelectCommand="SELECT DISTINCT [chest_pain_type] FROM [heartdataset]"></asp:SqlDataSource>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Rest Blood Pressure</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="blood_pressure" runat="server" CssClass="form-control" placeholder="Blood Pressure"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ControlToValidate="blood_pressure" ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div> 
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Blood suger</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList CssClass="form-control" ID="blood_suger" runat="server">
                                        <asp:ListItem Value ="True">
                                            Have
                                        </asp:ListItem>
                                        <asp:ListItem Value="False">
                                            I don't
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">rest electro</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList CssClass="form-control" ID="rest_electro" runat="server" DataSourceID="restelectroDS" DataTextField="rest_electro" 
                                        DataValueField="rest_electro"></asp:DropDownList>
                                    <asp:SqlDataSource ID="restelectroDS" runat="server" ConnectionString="<%$ ConnectionStrings:heartDiseaseConnectionString %>" 
                                        SelectCommand="SELECT DISTINCT [rest_electro] FROM [heartdataset] WHERE ([rest_electro] IS NOT NULL)"></asp:SqlDataSource>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">max heart rate</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="max_heart_rate" runat="server" CssClass="form-control" placeholder="max heart rate"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ControlToValidate="max_heart_rate" ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Exercice angina</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList CssClass="form-control" ID="exercice_angina" runat="server">
                                        <asp:ListItem Value="yes" Text ="yes">
                                            yes
                                        </asp:ListItem>
                                        <asp:ListItem Value="no" Text ="no">
                                            no
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Button CssClass="btn btn-primary col-4" Text="Predict" runat="server" OnClick="C_Coded_Algo" ></asp:Button><br />
                                <div class="col-4">
                                    <asp:Label ID="result" runat="server" Text="Prediction Value"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <%--Second form for predicting using MS-SQL Analysis Services--%>
                        <div class="col-md-6">
                            <br />
                            <h5>
                                MS-SQL Analysis Services
                            </h5>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Age</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="age_ms" runat="server" CssClass="form-control" placeholder="age"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ValidationGroup="group2" ControlToValidate="age_ms" ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">chest pain type</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList CssClass="form-control" ID="chest_ms" runat="server" DataSourceID="chestpainDS" DataTextField="chest_pain_type" 
                                        DataValueField="chest_pain_type"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Rest Blood Pressure</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="blood_press_ms" runat="server" CssClass="form-control" placeholder="Blood Pressure"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ValidationGroup="group2" ControlToValidate="blood_press_ms" ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div> 
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Blood suger</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList CssClass="form-control" ID="blood_suger_ms" runat="server">
                                        <asp:ListItem Value ="True">
                                            Have
                                        </asp:ListItem>
                                        <asp:ListItem Value="False">
                                            I don't
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">rest electro</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList CssClass="form-control" ID="rest_electro_ms" runat="server" DataSourceID="restelectroDS" DataTextField="rest_electro" 
                                        DataValueField="rest_electro"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">max heart rate</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="max_heart_rate_ms" runat="server" CssClass="form-control" placeholder="max heart rate"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:RequiredFieldValidator ValidationGroup="group2" ControlToValidate="max_heart_rate_ms" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Exercice angina</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList CssClass="form-control" ID="exercice_angina_ms" runat="server">
                                        <asp:ListItem Value="yes" Text ="yes">
                                            yes
                                        </asp:ListItem>
                                        <asp:ListItem Value="no" Text ="no">
                                            no
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Button CssClass="btn btn-primary col-4" ValidationGroup="group2" Text="Predict" runat="server" OnClick="MS_Algo" ></asp:Button><br />
                                <div class="col-4 align-content-center">
                                    <asp:Label ID="result2" runat="server" Text="Predition Value"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
        


        <%-- explaining --%>
        <div class="tab-pane fade" id="nav-explain" role="tabpanel" aria-labelledby="nav-profile-tab">
            <div class="container">
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th scope="col">prediction values</th>
                            <th scope="col">C# programmed $PROBABILITY</th>
                            <th scope="col">MS analysis services $PROBABILITY</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th scope="row">Positive</th>
                            <td><asp:Label ID="c_pos" runat="server" Text="Label"></asp:Label></td>
                            <td><asp:Label ID="ms_pos" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th scope="row">Nigative</th>
                            <td><asp:Label ID="c_neg" runat="server" Text="Label"></asp:Label></td>
                            <td><asp:Label ID="ms_neg" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="container">
                <div class="row">
                    <div class="col">
                        <h3 class="h3">
                            Explaining the algorithm
                        </h3>
                        <p class="text-md-left">
                            you can see the deferences between the two ways and the reasons is: 
                        </p>
                        <p>
                          1- in MS-SQL analysis services use Discretization, where the raw values of a numeric attribute (e.g., age) are replaced by interval labels (e.g., 0-10, 11-20, etc.). 
                        </p>
                        <p>
                            2- in my programmed algorithm i used normalization for numeric attribute and the two way are correct
                        </p>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/image/age.PNG" />
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/image/bayes.PNG" />
                    </div>
                </div>
            </div>
        </div>
        


        <%-- references --%>

        <div class="tab-pane fade" id="nav-contact" role="tabpanel" aria-labelledby="nav-contact-tab">
            <div class="container">
                <div class="row">
                    <br />
                    <a class="card-link" href="https://www.codeproject.com/Articles/318126/Naive-Bayes-Classifier">1- Nave Bayes Classifier, by Milan Solanki</a>
                </div>
            </div>
        </div>
    </div>
    </div>
</body>
</html>
