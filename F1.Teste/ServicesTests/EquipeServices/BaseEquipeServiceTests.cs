using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Services;

namespace F1.Teste.ServicesTests.EquipeServices;

public abstract class BaseEquipeServiceTests
{
    protected readonly Mock<IMapper> mockMapper;
    protected readonly Mock<IEquipeQuery> mockQuery;
    protected readonly Mock<IRepositoryBase<Equipe>> mockRepo;
    protected readonly Mock<IUnitOfWork> mockUow;

    protected readonly CreateEquipeService createService;
    protected readonly ReadEquipeService readService;
    protected readonly UpdateEquipeService updateService;
    protected readonly DeleteEquipeService deleteService;

    public BaseEquipeServiceTests()
    {
        mockMapper = new Mock<IMapper>();
        mockQuery = new Mock<IEquipeQuery>();
        mockRepo = new Mock<IRepositoryBase<Equipe>>();
        mockUow = new Mock<IUnitOfWork>();

        createService = new CreateEquipeService(mockMapper.Object, mockQuery.Object,
            mockRepo.Object, mockUow.Object);
        readService = new ReadEquipeService(mockMapper.Object, mockQuery.Object);
        updateService = new UpdateEquipeService(mockMapper.Object, mockQuery.Object,
            mockRepo.Object, mockUow.Object);
        deleteService = new DeleteEquipeService(mockQuery.Object, mockRepo.Object,
            mockUow.Object);
    }
}
