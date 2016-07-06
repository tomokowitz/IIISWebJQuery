Imports IIISWeb_jQueryGrid.Models

Public Class ReadingsRepository
    Implements IReadingsRepository

    Public Function ReadingRepository()


    End Function

    Public Sub Create(userID As Short, reading As ReadingModel)



    End Sub

    Public Function GetReading(ReadingID As Integer) As ReadingModel
        Dim reading As ReadingModel

        Return reading
    End Function

    Public Function GetAllReadings() As IOrderedQueryable(Of ReadingModel)

        Try
            Dim model As ReadingModel = New ReadingModel()
            Dim readings As IOrderedQueryable(Of ReadingModel) = model.GetAllReadings()

            Return readings

        Catch ex As Exception
            Utilities.WriteError(ex, "Reading Repository error")
        End Try
    End Function

    Private Sub IReadingsRepository_Create(userId As Short, reading As ReadingModel) Implements IReadingsRepository.Create
        Throw New NotImplementedException()
    End Sub

    Private Function IReadingsRepository_GetReading(ReadingID As Integer) As ReadingModel Implements IReadingsRepository.GetReading
        Throw New NotImplementedException()
    End Function

    Private Function IReadingsRepository_GetAllReadings() As IEnumerable(Of ReadingModel) Implements IReadingsRepository.GetAllReadings
        Throw New NotImplementedException()
    End Function
End Class



'Using System.Collections.Generic;
'Using System.Data;
'Using System.Linq;
'Using MileageStats.Model;

'Namespace MileageStats.Data.SqlCe.Repositories
'{
'    Public Class VehicleRepository :  BaseRepository, IVehicleRepository
'    {
'        Public VehicleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
'        {
'        }

'        Public void Create(int userId, Vehicle vehicle)
'        {
'            vehicle.UserId = userId;
'            this.GetDbSet<Vehicle>().Add(vehicle);
'            this.UnitOfWork.SaveChanges();
'        }

'        Public Vehicle GetVehicle(int userId, int vehicleId)
'        {
'            Return this.GetDbSet < Vehicle > ()
'        .Include("Fillups")
'        .Include("Reminders")
'        .Where(v >= v.VehicleId == vehicleId && v.UserId == userId)
'        .Single();
'        }

'        Public IEnumerable<Vehicle> GetVehicles(int userId)
'        {
'            Return this.GetDbSet < Vehicle > ()
'        .Include("Fillups")
'        .Include("Reminders")
'        .Where(v >= v.UserId == userId)
'        .ToList();
'        }

'        Public void Update(Vehicle updatedVehicle)
'        {
'            Vehicle vehicleToUpdate =
'                this.GetDbSet < Vehicle > ()
'        .Where(v >= v.VehicleId == updatedVehicle.VehicleId)
'        .First();

'            vehicleToUpdate.Name = updatedVehicle.Name;
'            vehicleToUpdate.Year = updatedVehicle.Year;
'            vehicleToUpdate.MakeName = updatedVehicle.MakeName;
'            vehicleToUpdate.ModelName = updatedVehicle.ModelName;
'            vehicleToUpdate.SortOrder = updatedVehicle.SortOrder;
'            vehicleToUpdate.PhotoId = updatedVehicle.PhotoId;

'            this.SetEntityState(vehicleToUpdate, vehicleToUpdate.VehicleId == 0
'                                                     ? EntityState.Added
'                                                      EntityState.Modified);
'            this.UnitOfWork.SaveChanges();
'        }

'        Public void Delete(int vehicleId)
'        {
'            Vehicle vehicle =
'                this.GetDbSet < Vehicle > ()
'        .Where(v >= v.VehicleId == vehicleId)
'        .Single();

'            this.GetDbSet<Vehicle>().Remove(vehicle);

'            this.UnitOfWork.SaveChanges();
'        }
'    }
'}
