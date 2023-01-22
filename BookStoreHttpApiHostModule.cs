...

namespace BookStore;

[DependsOn(...)]
public class BookStoreHttpApiHostModule : AbpModule
{
    ...

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddDevExpressControls();
        context.Services.AddSession();

        context.Services.AddScoped<IObjectDataSourceInjector, ObjectDataSourceInjector>();
        context.Services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
        context.Services.TryAddSingleton(typeof(IScopedServiceProvider<ReportingDataSourceService>), typeof(ScopedServiceProvider<ReportingDataSourceService>));

        context.Services.ConfigureReportingServices(configurator => {
            configurator.ConfigureReportDesigner(designerConfigurator => {
                designerConfigurator.RegisterObjectDataSourceWizardTypeProvider<ObjectDataSourceWizardCustomTypeProvider>();
            });
            configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
                viewerConfigurator.UseCachedReportSourceBuilder();
            });
            context.Services.AddScoped<IReportProviderAsync, CustomReportProviderAsync>();

            configurator.UseAsyncEngine();
        });


        ...

        context.Services.BuildServiceProvider();
    }

    ...

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        ...

        DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Expressions;
        app.UseDevExpressControls();
        app.UseCorrelationId();

        ... Endpoints
    }
}
