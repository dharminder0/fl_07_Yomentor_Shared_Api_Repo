using Core.Business.Entites.DataModels;
using Core.Business.Services.Abstract;
using Core.Data.Repositories.Abstract;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class PushNotificationBackgroundService : BackgroundService {
    private readonly IServiceProvider _serviceProvider;

    private const string SERVICE_NAME = "PushNotificationBackgroundService";
    private readonly ILogger _logger;

    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
 

    public PushNotificationBackgroundService(ILogger<PushNotificationBackgroundService> logger,  IUserRepository userRepository, IUserService userService) {
        _logger = logger;
        _userService = userService;
        
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {

            try {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                await PushNotifications();
            } catch (Exception ex) {
                _logger.LogError($@"The Website is Down {SERVICE_NAME} .", ex.Message);
            

            } finally {
                // await Task.Delay(1000 * 5, stoppingToken);
                _logger.LogInformation($"service stopped:");
             
            }

        }
    }

    public async Task PushNotifications() {
        //using (var  scope = _serviceProvider.CreateScope()) {
        //    var _appointment = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
        //}


        try {
           
            // business for the service
            _logger.LogInformation($"service started:");

            var response =  await _userService.GetPushNotification(); 
           

        } catch (Exception ex) {
            _logger.LogError($@"The Website is Down {SERVICE_NAME} .", ex.Message);
            
        }
        await Task.FromResult("Done");
        //return Task.CompletedTask;
    }

   


}