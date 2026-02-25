using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Services;

namespace F1.Teste.ServicesTests.GPServices;

public abstract class BaseGPServiceTests
{
    protected readonly Mock<IMapper> mockMapper;
    protected readonly Mock<IGrandePremioQuery> mockQuery;
    protected readonly Mock<IPilotoQuery> mockPilotoQuery;
    protected readonly Mock<IRepositoryBase<GrandePremio>> mockRepo;
    protected readonly Mock<IUnitOfWork> mockUow;

    protected readonly CreateGrandePremioService createService;
    protected readonly ReadGrandePremioService readService;
    protected readonly UpdateGrandePremioService updateService;
    protected readonly DeleteGrandePremioService deleteService;

    public BaseGPServiceTests()
    {
        mockMapper = new Mock<IMapper>();
        mockQuery = new Mock<IGrandePremioQuery>();
        mockPilotoQuery = new Mock<IPilotoQuery>();
        mockRepo = new Mock<IRepositoryBase<GrandePremio>>();
        mockUow = new Mock<IUnitOfWork>();

        createService = new CreateGrandePremioService(mockMapper.Object, mockPilotoQuery.Object,
            mockQuery.Object, mockRepo.Object, mockUow.Object);
        readService = new ReadGrandePremioService(mockMapper.Object, mockQuery.Object);
        updateService = new UpdateGrandePremioService(mockMapper.Object, mockPilotoQuery.Object,
            mockQuery.Object, mockRepo.Object, mockUow.Object);
        deleteService = new DeleteGrandePremioService(mockQuery.Object, mockRepo.Object, 
            mockUow.Object);
    }
}
