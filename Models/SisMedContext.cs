using System.Data.Entity;

namespace SisMedApi.Models
{
    public class SisMedContext : DbContext
    {
        public SisMedContext()
            : base("name=SisMedContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<CLIENTE> CLIENTES { get; set; }

        public DbSet<CLIENTE_CONTATO> CLIENTE_CONTATO { get; set; }

        public DbSet<CLIENTE_ENDERECO> CLIENTE_ENDERECO { get; set; }

        public DbSet<CIDADE> CIDADEs { get; set; }

        public DbSet<PROFISSIONAL_ENDERECO> PROFISSIONAL_ENDERECO { get; set; }

        public DbSet<PROFISSIONAL> PROFISSIONALs { get; set; }

        public DbSet<SisMedApi.Models.FORNECEDOR> FORNECEDORs { get; set; }

        public DbSet<SisMedApi.Models.FORNECEDOR_ENDERECO> FORNECEDOR_ENDERECO { get; set; }

        public DbSet<SisMedApi.Models.FORNECEDOR_CONTATO> FORNECEDOR_CONTATO { get; set; }

        public DbSet<SisMedApi.Models.FORNECEDOR_TIPO> TIPO_FORNECEDOR { get; set; }

        public DbSet<SisMedApi.Models.TIPO_PRODUTO> TIPO_PRODUTO { get; set; }

        public DbSet<SisMedApi.Models.PRODUTO_FORNECEDORES> PRODUTO_FORNECEDORES { get; set; }

        public DbSet<SisMedApi.Models.PRODUTO> PRODUTOes { get; set; }

        public DbSet<SisMedApi.Models.REGIAO_CORPO> REGIAO_CORPO { get; set; }

        public DbSet<SisMedApi.Models.EQUIPAMENTO> EQUIPAMENTOes { get; set; }

        public DbSet<SisMedApi.Models.PROCEDIMENTO> PROCEDIMENTOes { get; set; }

        public DbSet<SisMedApi.Models.PROCEDIMENTO_EQUIPAMENTO> PROCEDIMENTO_EQUIPAMENTO { get; set; }

        public DbSet<SisMedApi.Models.PROCEDIMENTO_REGIAO_CORPO> PROCEDIMENTO_REGIAO_CORPO { get; set; }

        public DbSet<SisMedApi.Models.SALA> SALAs { get; set; }

        public DbSet<SisMedApi.Models.SALA_HORARIO> SALA_HORARIO { get; set; }

        public DbSet<SisMedApi.Models.PROFISSIONAL_HORARIO> PROFISSIONAL_HORARIO { get; set; }

        public DbSet<SisMedApi.Models.Appointments> Appointments { get; set; }

        public DbSet<SisMedApi.Models.CONVENIO> CONVENIOs { get; set; }

        public DbSet<SisMedApi.Models.CONVENIOS_CLIENTE> CONVENIOS_CLIENTE { get; set; }

        public DbSet<SisMedApi.Models.INDICACAO> INDICACAOs { get; set; }

        public DbSet<SisMedApi.Models.CONVENIO_CONTATO> CONVENIO_CONTATO { get; set; }

        public DbSet<SisMedApi.Models.PROCEDIMENTO_REGIAO_COMISSAO> PROCEDIMENTO_REGIAO_COMISSAO { get; set; }

        public DbSet<SisMedApi.Models.PROFISSIONAL_COMISSAO> PROFISSIONAL_COMISSAO { get; set; }

        public DbSet<SisMedApi.Models.PROFISSIONAL_RECEITA> PROFISSIONAL_RECEITA { get; set; }

        public DbSet<SisMedApi.Models.ANAMNESE> ANAMNESEs { get; set; }

        public DbSet<SisMedApi.Models.ATENDIMENTO> ATENDIMENTOes { get; set; }

        public DbSet<SisMedApi.Models.USUARIO> USUARIOs { get; set; }

        public DbSet<SisMedApi.Models.PROTOCOLO> PROTOCOLOes { get; set; }

        public DbSet<SisMedApi.Models.ATENDIMENTO_ANAMNESE> ATENDIMENTO_ANAMNESE { get; set; }

        public DbSet<SisMedApi.Models.ATENDIMENTO_PRODUTO> ATENDIMENTO_PRODUTO { get; set; }

        public DbSet<SisMedApi.Models.ATENDIMENTO_PROTOCOLO> ATENDIMENTO_PROTOCOLO { get; set; }

        public DbSet<SisMedApi.Models.ATENDIMENTO_RECEITA> ATENDIMENTO_RECEITA { get; set; }

        public DbSet<SisMedApi.Models.MEDIDA> MEDIDAs { get; set; }

        public DbSet<SisMedApi.Models.ATENDIMENTO_MEDIDA> ATENDIMENTO_MEDIDA { get; set; }

        public DbSet<SisMedApi.Models.PROCEDIMENTO_PRODUTO> PROCEDIMENTO_PRODUTO { get; set; }

        public DbSet<SisMedApi.Models.PROCEDIMENTO_REGIAO_PRODUTO> PROCEDIMENTO_REGIAO_PRODUTO { get; set; }

        public DbSet<SisMedApi.Models.CONVENIO_PROCEDIMENTO> CONVENIO_PROCEDIMENTO { get; set; }

        public DbSet<SisMedApi.Models.CONVENIO_DEDUCAO> CONVENIO_DEDUCAO { get; set; }

        public DbSet<SisMedApi.Models.VENDA> VENDAs { get; set; }

        public DbSet<SisMedApi.Models.VENDA_ITEM> VENDA_ITEM { get; set; }

        public DbSet<SisMedApi.Models.VENDA_PGTOS> VENDA_PGTOS { get; set; }

        public DbSet<SisMedApi.Models.CAIXA> CAIXAs { get; set; }

        public DbSet<SisMedApi.Models.CAIXA_ITEM> CAIXA_ITEM { get; set; }

        public DbSet<SisMedApi.Models.CATEGORIA> CATEGORIAs { get; set; }

        public DbSet<SisMedApi.Models.SUB_CATEGORIA> SUB_CATEGORIA { get; set; }

        public DbSet<SisMedApi.Models.UNIDADE> UNIDADEs { get; set; }

        public DbSet<SisMedApi.Models.CENTRO_CUSTO> CENTRO_CUSTO { get; set; }

        public DbSet<SisMedApi.Models.BANCO> BANCOes { get; set; }

        public DbSet<CONTA> CONTAs { get; set; }

        public DbSet<SisMedApi.Models.DOCUMENTO_FINANCEIRO> DOCUMENTO_FINANCEIRO { get; set; }

        public DbSet<SisMedApi.Models.DOCUMENTO_EXTRATO> DOCUMENTO_EXTRATO { get; set; }

        public DbSet<SevenMedicalApi.Models.ATENDIMENTO_PROCEDIMENTO> ATENDIMENTO_PROCEDIMENTO { get; set; }

        public DbSet<SevenMedicalApi.Models.TRANSFERENCIA> TRANSFERENCIAs { get; set; }

        public DbSet<SevenMedicalApi.Models.ATENDIMENTO_TRATAMENTO> ATENDIMENTO_TRATAMENTO { get; set; }

        public DbSet<SevenMedicalApi.Models.FLUXO_ATENDIMENTO> FLUXO_ATENDIMENTO { get; set; }

        public DbSet<SevenMedicalApi.Models.TIPO_SERVICO> TIPO_SERVICO { get; set; }

        public DbSet<SevenMedicalApi.Models.MEDICAMENTO> MEDICAMENTOes { get; set; }

        public DbSet<SevenMedicalApi.Models.DISEASES> DISEASES { get; set; }

        public DbSet<SevenMedicalApi.Models.PROFISSIONAL_UNIDADE> PROFISSIONAL_UNIDADE { get; set; }

        public DbSet<SevenMedicalApi.Models.USUARIO_PERMISSAO> USUARIO_PERMISSAO { get; set; }

        public DbSet<SevenMedicalApi.Models.CARTAO> CARTAOs { get; set; }

        public DbSet<SevenMedicalApi.Models.CARTAO_FAIXA_DESCONTO> CARTAO_FAIXA_DESCONTOes { get; set; }

        public DbSet<SevenMedicalApi.Models.V_LOCALIDADES> V_LOCALIDADES { get; set; }

        public DbSet<SevenMedicalApi.Models.CONTROLE_SERVICO_REALIZAR> CONTROLE_SERVICO_REALIZAR { get; set; }

        public DbSet<SevenMedicalApi.Models.V_COMISSOES> V_COMISSOES { get; set; }

        public DbSet<SisMedApi.Models.REGIAO_CORPO_ALERTA> REGIAO_CORPO_ALERTA { get; set; }

        public DbSet<SevenMedicalApi.Models.UNIDADE_IMPOSTO> UNIDADE_IMPOSTO { get; set; }

        public DbSet<SevenMedicalApi.Models.ACESSO> ACESSOes { get; set; }

        public DbSet<SevenMedicalApi.Models.PERFIL> PERFILs { get; set; }

        public DbSet<SevenMedicalApi.Models.PERFIL_ACESSO> PERFIL_ACESSO { get; set; }

        public DbSet<SevenMedicalApi.Models.USUARIO_ACESSO> USUARIO_ACESSO { get; set; }

        public DbSet<SisMedApi.Models.CRM_LIGACAO> CRM_LIGACAO { get; set; }

        public DbSet<SisMedApi.Models.APPOINTMENTS_REGIAO_CORPO> APPOINTMENTS_REGIAO_CORPO { get; set; }

        public DbSet<SevenMedicalApi.Models.ENTRADA_PRODUTO> ENTRADA_PRODUTO { get; set; }

        public DbSet<SevenMedicalApi.Models.FARMACIA_SATELITE> FARMACIA_SATELITE { get; set; }

        public DbSet<SevenMedicalApi.Models.ENTRADA_PRODUTO_ITEM> ENTRADA_PRODUTO_ITEM { get; set; }

        public DbSet<SevenMedicalApi.Models.PRODUTO_CONVERSAO> PRODUTO_CONVERSAO { get; set; }

        public DbSet<SevenMedicalApi.Models.UNIDADE_SOCIO> UNIDADE_SOCIO { get; set; }

        public DbSet<SevenMedicalApi.Models.UNIDADE_FARMACIA> UNIDADE_FARMACIA { get; set; }

        public DbSet<SevenMedicalApi.Models.FARMACIA_ESTOQUE> FARMACIA_ESTOQUE { get; set; }

        public DbSet<SevenMedicalApi.Models.ESTOQUE> ESTOQUEs { get; set; }

        public DbSet<SevenMedicalApi.Models.UNIDADE_PRODUTO> UNIDADE_PRODUTO { get; set; }

        public DbSet<SisMedApi.Models.USUARIO_PROFISSIONAL> USUARIO_PROFISSIONAL { get; set; }
    }
}