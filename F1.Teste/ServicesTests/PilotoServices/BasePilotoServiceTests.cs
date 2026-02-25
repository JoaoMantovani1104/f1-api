using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Services;

namespace F1.Teste.ServicesTests.PilotoServices;

public class BasePilotoServiceTests
{
    protected readonly Mock<IMapper> mockMapper;
    protected readonly Mock<IPilotoQuery> mockQuery;
    protected readonly Mock<IEquipeQuery> mockEquipeQuery;
    protected readonly Mock<IRepositoryBase<Piloto>> mockRepo;
    protected readonly Mock<IUnitOfWork> mockUow;

    protected readonly CreatePilotoService createService;
    protected readonly ReadPilotoService readService;
    protected readonly UpdatePilotoService updateService;
    protected readonly DeletePilotoService deleteService;

    public BasePilotoServiceTests()
    {
        mockMapper = new Mock<IMapper>();
        mockQuery = new Mock<IPilotoQuery>();
        mockEquipeQuery = new Mock<IEquipeQuery>();
        mockRepo = new Mock<IRepositoryBase<Piloto>>();
        mockUow = new Mock<IUnitOfWork>();

        createService = new CreatePilotoService(mockMapper.Object, mockEquipeQuery.Object,
            mockQuery.Object, mockRepo.Object, mockUow.Object);
        readService = new ReadPilotoService(mockMapper.Object, mockQuery.Object);
        updateService = new UpdatePilotoService(mockMapper.Object, mockEquipeQuery.Object, 
            mockQuery.Object, mockRepo.Object, mockUow.Object);
        deleteService = new DeletePilotoService(mockQuery.Object, mockRepo.Object,
            mockUow.Object);
    }
}
