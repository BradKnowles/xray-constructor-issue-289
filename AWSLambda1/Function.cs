using Amazon.Lambda.Core;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using Amazon.XRay.Recorder.Handlers.AwsSdk;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambda1;

public class Function
{
    private readonly AWSXRayRecorder _recorder;

    public Function()
    {
        AWSSDKHandler.RegisterXRayForAllServices();
        AWSXRayRecorder.InitializeInstance();

        AWSXRayRecorder.RegisterLogger(Amazon.LoggingOptions.Console);

        _recorder = AWSXRayRecorder.Instance;
        _recorder.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;

        _recorder.BeginSubsegment("Constructor");
        Console.WriteLine("Inside constructor subsegment");
        _recorder.EndSubsegment();
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(string input, ILambdaContext context)
    {
        _recorder.BeginSubsegment("Converting to uppercase");
        var value = input.ToUpper();
        _recorder.EndSubsegment();

        return value;
    }
}
