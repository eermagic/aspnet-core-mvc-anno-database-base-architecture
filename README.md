# aspnet-core-mvc-anno-database-base-architecture
[ASP.NET Core MVC][Vue3][Dapper] 後端資料庫底層架構建置 #CH3

在前兩個教學章節我完成了新增、修改、刪除及查詢的資料庫語法，而我教學的語法會放在第一層全部展示，這適合教學好理解，但不適合專案使用，不然會大量出現相同功能的程式碼，而在這個章節會做一次後端資料庫物件導向化的底層架構建置。

底層架構建置是一種方法抽離的過程，將相同功能的程式碼抽離至方法內，只提供方法呼叫，可以簡化重複撰寫的程式碼。

擁有底層架構的專案，在撰寫程式碼就會減短許多，這也是物件導向的精神，將相同功能封裝起來，只提供對外呼叫的方法。

接下來這個範例會將前面兩個章節的範例，將資料庫執行的共用的部份，封裝到底層的類別庫，實現資料庫底層類別庫的製作。

在開始之前，建議你先實作完[第一篇](https://blog.hungwin.com.tw/aspnet-mvc-anno-backstage-query/)與[第二篇](https://blog.hungwin.com.tw/aspnet-mvc-anno-backstage-modify/)的教學範例，會比較好理解底層架構的建置過程。

[完整教學文章](https://blog.hungwin.com.tw/aspnet-core-mvc-anno-database-base-architecture/)

