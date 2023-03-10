var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
List<Person> personlist = new List<Person>();
app.Run(async (context) =>
{
app.Logger.LogInformation($"Path: { context.Request.Path }");
	//if (context.Request.Path == "/favicon.ico") context.Response.StatusCode = 404;
	switch(context.Request.Path)
	{
		case "/api/add":
			//context.Request.ContentType = "application/json";
			Person dude = await context.Request.ReadFromJsonAsync<Person>();
			foreach (Person person in personlist)
				if (dude.id == person.id) dude.id++;
			personlist.Add(dude);
			await context.Response.WriteAsJsonAsync(dude);
			break;
		default:
			context.Response.ContentType = "text/html";
			await context.Response.SendFileAsync("index.html");
			//Person johnn = new Person("12333","1");
			//await context.Response.WriteAsJsonAsync(johnn);
			break;

	}

});
app.Run();
public class Person
{
	public int id { get; set; }
	public string name { get; set; }
	public string s_name { get; set; }

	public Person(string name, string s_name) { this.name = name; this.s_name = s_name; }
	public override string ToString()
	{
		return $"Name: {name} Second Name:{s_name}";
	}
	//Пока работаем только с name и id, потом добвавить новые переменные
}


