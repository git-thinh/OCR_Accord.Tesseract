May 27, 2015
This is a port of the the Microsoft assembly for the .Net 4.0 Dynamic language functionality.
The binary is available on [NuGet][1].  **Install-Package System.Linq.Dynamic.Library**
[1]: https://www.nuget.org/packages/System.Linq.Dynamic.Library/ "NuGet - Dynamic Linq Library"
[2]: http://dynamiclinq.azurewebsites.net
--------------------------------------------------------------------------------------------------
var userById = qry.Where("Id=@0", testList[10].Id);
var userByUserName = qry.Where("UserName=\"User5\"");
var nullProfileCount = qry.Where("Profile=null");
var userByFirstName = qry.Where("Profile!=null && Profile.FirstName=@0", testList[1].Profile.FirstName);

            var orderById = qry.OrderBy("Id");
            var orderByIdDesc = qry.OrderBy("Id DESC");
            var orderByAge = qry.OrderBy("Profile.Age");
            var orderByAgeDesc = qry.OrderBy("Profile.Age DESC");
            var orderByComplex = qry.OrderBy("Profile.Age, Id");
            var orderByComplex2 = qry.OrderBy("Profile.Age DESC, Id");

var orderById = qry.SelectMany("Roles.OrderBy(Name)").Select("Name");
var expected = qry.SelectMany(x => x.Roles.OrderBy(y => y.Name)).Select( x => x.Name);
var orderByIdDesc = qry.SelectMany("Roles.OrderByDescending(Name)").Select("Name");
var expectedDesc = qry.SelectMany(x => x.Roles.OrderByDescending(y => y.Name)).Select(x => x.Name);


            Helper.ExpectException<ParseException>(() => qry.OrderBy("Bad=3"));
            Helper.ExpectException<ParseException>(() => qry.Where("Id=123"));
            Helper.ExpectException<ArgumentNullException>(() => DynamicQueryable.OrderBy(null, "Id"));
            Helper.ExpectException<ArgumentNullException>(() => qry.OrderBy(null));
            Helper.ExpectException<ArgumentException>(() => qry.OrderBy(""));
            Helper.ExpectException<ArgumentException>(() => qry.OrderBy(" "));


    IEnumerable rangeResult = range.AsQueryable().Select("it * it");
    var userNames = qry.Select("UserName");
    var userFirstName = qry.Select("new (UserName, Profile.FirstName as MyFirstName)");
    var userRoles = qry.Select("new (UserName, Roles.Select(Id) AS RoleIds)");
            Helper.ExpectException<ParseException>(() => qry.Select("Bad"));
            Helper.ExpectException<ParseException>(() => qry.Select("Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.Select("new Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.Select("new (Id, UserName"));
            Helper.ExpectException<ParseException>(() => qry.Select("new (Id, UserName, Bad)"));

            Helper.ExpectException<ArgumentNullException>(() => DynamicQueryable.Select(null, "Id"));
            Helper.ExpectException<ArgumentNullException>(() => qry.Select(null));
            Helper.ExpectException<ArgumentException>(() => qry.Select(""));
            Helper.ExpectException<ArgumentException>(() => qry.Select(" "));









