<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Audit_myfiles.aspx.cs" Inherits="user_Audit_myfiles" %>

<!DOCTYPE html>



<html lang="en">
  <head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <title>LEVER:Secure  Deduplication</title>
  <!-- web fonts -->
  <link href="/../fonts.googleapis.com/css?family=Poppins:100,300,400,500,600,700,800,900&amp;display=swap" rel="stylesheet">
  <link href="/../fonts.googleapis.com/css?family=Hind&amp;display=swap" rel="stylesheet">
  <!-- //web fonts -->
    <!-- Template CSS -->
    <link rel="stylesheet" href="assets/css/style-starter.css">
     <style>
         .imgg {
  border-radius: 50%;
}
     </style>
  </head>

  <body>
      <form id="f1" runat="server">

<!-- Top Menu 1 -->
<section class="w3l-top-menu-1">
	<div class="top-hd">
		<div class="container">
	<header class="row">
		<%--<div class="social-top col-sm-6 col-6 pl-0">
			
			
		</div>
		<div class="accounts col-sm-6 col-6 pr-0">
				<li class="top_li1"><a href="#">Login</a></li>
				<li class="top_li2"><a href="#">Register</a></li>
		</div>--%>
		
	</header>
</div>
</div>
</section>
<!-- //Top Menu 1 -->
<section class="w3l-bootstrap-header">
  <nav class="navbar navbar-expand-lg navbar-light py-lg-2 py-2">
    <div class="container">
      <a class="navbar-brand" href="index.html"><span>Secure: </span>Deduplication</a>
      <!-- if logo is image enable this   
    <a class="navbar-brand" href="#index.html">
        <img src="image-path" alt="Your logo" title="Your logo" style="height:35px;" />
    </a> -->
      <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent"
        aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon fa fa-bars"></span>
      </button>

     <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <ul class="navbar-nav mx-auto">
          <li class="nav-item">
            <a class="nav-link" href="user_home.aspx">Home</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="upload_file.aspx">Upload Files</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="download_files.aspx">Download Files</a>
          </li>
         <li class="nav-item">
            <a class="nav-link" href="view_feedbacks.aspx">Feedbacks</a>
          </li>
        <li class="nav-item">
            <a class="nav-link" href="complaints.aspx">My complaints</a>
          </li>
        
        
          <li class="nav-item">
            <a class="nav-link" href="../Index/index.aspx">Logout</a>
          </li>
        </ul>
        <%--<p>For Support Call<span class="fa fa-headphones pl-1"></span><br><a href="tel:900-369-8527">900-369-8527</a></p>--%>
      </div>
    </div>
  </nav>
</section>
<section class="w3l-about-breadcrum">
  <div class="breadcrum-bg py-sm-5 py-4">
    <div class="container py-lg-3 py-2">
      <h2 ALIGN="CENTER">Audit MyFiles</h2>
     
    </div>
  </div>
</section>
<!-- content-with-photo4 block -->
<section class="w3l-content-with-photo-4" id="about">
    <div id="content-with-photo4-block" class="pt-5"> 
        <div class="container py-md-3">
            <div class="cwp4-two row">
                <div class="row">
                  <br />
                <h2>Non Duplicated Files </h2>
                <br />
                <asp:DataList ID="DataList2" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" CssClass="table table-hover" OnDeleteCommand="DataList2_DeleteCommand" OnUpdateCommand="DataList2_UpdateCommand">
                    <ItemTemplate>
                        <table class="w-100">
                            <tr>
                                <td style="text-align: center" class="auto-style5">&nbsp;&nbsp;&nbsp;
                                    <asp:Image ID="Image1" runat="server" Height="115px" ImageUrl="~/Auditor/assets/images/folder.jpg" Width="131px" />
                                    &nbsp;
                                    <asp:Label ID="Label10" runat="server" Text='<%# bind("fid") %>' Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; vertical-align: middle" class="auto-style5" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label7" runat="server" Text='<%# bind("filename") %>' Font-Bold="true"></asp:Label>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: center; vertical-align: middle" class="auto-style5">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label8" runat="server" Text="Full Duplications  :"></asp:Label>
                                    &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" Font-Italic="True" Font-Underline="True" ForeColor="#0033CC" CommandName="update">View Details</asp:LinkButton>
                                </td>
                                 <td style="text-align: center; vertical-align: middle" class="auto-style3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style5" style="text-align: center; vertical-align: middle">Partial Duplications:<br />
                                    <asp:LinkButton ID="LinkButton2" runat="server" Font-Italic="True" Font-Underline="True" ForeColor="#0033CC" CommandName="delete">View Details</asp:LinkButton>
                                </td>
                                <td class="auto-style3" style="text-align: center; vertical-align: middle">
                                    <asp:Label ID="Label11" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>
              
            </div>
        </div>
    </div>
</section>

<script src="assets/js/jquery-3.3.1.min.js"></script>
<!-- //footer-28 block -->

<script>
    $(function () {
        $('.navbar-toggler').click(function () {
            $('body').toggleClass('noscroll');
        })
    });
</script>
<!-- jQuery first, then Popper.js, then Bootstrap JS -->
<script src="https://code.jquery.com/jquery-3.4.1.slim.min.js"
  integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous">
</script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js"
  integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous">
</script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"
  integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous">
</script>

<!-- Template JavaScript -->
<script src="assets/js/all.js"></script>
<!-- Smooth scrolling -->
<!-- <script src="assets/js/smoothscroll.js"></script> -->
<script src="assets/js/owl.carousel.js"></script>

<!-- script for -->
<script>
    $(document).ready(function () {
        $('.owl-one').owlCarousel({
            loop: true,
            margin: 0,
            nav: true,
            responsiveClass: true,
            autoplay: false,
            autoplayTimeout: 5000,
            autoplaySpeed: 1000,
            autoplayHoverPause: false,
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                480: {
                    items: 1,
                    nav: false
                },
                667: {
                    items: 1,
                    nav: true
                },
                1000: {
                    items: 1,
                    nav: true
                }
            }
        })
    })
</script>
<!-- //script -->
      </form>
</body>

</html>