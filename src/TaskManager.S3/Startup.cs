using System.Diagnostics.CodeAnalysis;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using LocalStack.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.S3.Configs;
using TaskManager.S3.Model.Storage;

namespace TaskManager.S3;

[ExcludeFromCodeCoverage]
public static class Startup
{
    public static void AddStorageStartup(this IServiceCollection services, IConfiguration configuration)
    {
        var awsSettingsSection = configuration
            .GetSection("AwsS3")
            .Get<AwsS3Configuration>();

        services.Configure<AwsS3Configuration>(option =>
        {
            option.BucketName = awsSettingsSection?.BucketName;
            option.FolderPathName = awsSettingsSection?.FolderPathName;
        });

        var awsConfigs = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(awsSettingsSection.AccessKey, awsSettingsSection.SecretKey),
            Region = RegionEndpoint.USEast1
        };

        /** Para usar o S3 localmente
         * Em produção, a propriedade UseLocalStack no appSettings deve ser 'false'
         */
        services.AddLocalStack(configuration);

        services.AddDefaultAWSOptions(awsConfigs);

        services.AddAwsService<IAmazonS3>();

        services.AddScoped<IStorageService, StorageService>();
    }
}