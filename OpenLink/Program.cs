using OpenLink.Services;
using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.VisualBasic;
using Python.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

static void GetStringFromPy()
{
    // print all the files within this dir
    
    PythonEngine.Exec("from llmservice import example; example('hello')");

}

         
         
            
        


/*await RunChatSessionAsync();*/
LLMService.GetStringFromPy();
        





/*
# pragma warning disable SKEXP0010
var kernelBuilder = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: "phi3",
                apiKey: null,
                endpoint: new Uri("http://localhost:11434")
                );
var kernel = kernelBuilder.Build();
# pragma warning restore SKEXP0010
*/

app.Run();
