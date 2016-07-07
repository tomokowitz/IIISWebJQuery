<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width" />
    <title>@ViewData("Title")</title>
   
</head>
<body>
    @RenderBody()

   
    @RenderSection("scripts", required:=False)
</body>
</html>

