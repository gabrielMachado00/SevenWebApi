using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;
using System.Data.SqlClient;
using System.Text;

namespace SisMedApi.Controllers
{
    public class DOCUMENTO_FINANCEIRODTO
    {
        public int ID_DOCUMENTO { get; set; }
        public string TIPO { get; set; }
        public DateTime DATA_ATUALIZACAO { get; set; }
        public string LOGIN { get; set; }
        public Decimal VALOR { get; set; }
        public DateTime DATA_VENCIMENTO { get; set; }
        public string NUM_DOCUMENTO { get; set; }
        public Nullable<int> ID_CENTRO_CUSTO { get; set; }
        public Nullable<int> ID_SUB_CATEGORIA { get; set; }
        public string STATUS { get; set; }
        public Nullable<int> ID_CLIENTE { get; set; }
        public Nullable<int> ID_ITEM_CAIXA { get; set; }
        public Nullable<int> ID_FORNECEDOR { get; set; }
        public Decimal SALDO { get; set; }
        public string CENTRO_CUSTO { get; set; }
        public string SUB_CATEGORIA { get; set; }
        public string DESC_STATUS { get; set; }
        public string CLIENTE { get; set; }
        public string CATEGORIA { get; set; }
        public string DESC_TIPO { get; set; }
        public string FORNECEDOR { get; set; }
        public Nullable<int> ID_EQUIPAMENTO { get; set; }
        public string EQUIPAMENTO { get; set; }
        public Nullable<int> ID_CONTA { get; set; }
        public Nullable<int> ID_CATEGORIA { get; set; }
        public string CONTA { get; set; }
        public Nullable<int> TIPO_PGTO { get; set; }
        public Nullable<int> NUM_PARCELA { get; set; }
        public string DESC_TIPO_PGTO { get; set; }
        public string BANDEIRA_CARTAO { get; set; } 
        public Nullable<DateTime> DATA_QUITACAO { get; set; }
        public string OBS { get; set; }
        public string UNIDADE { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public string PROFISSIONAL { get; set; }
        public Nullable<int> ID_CARTAO { get; set; }
        public Nullable<int> CV_CARTAO { get; set; }
        public Nullable<int> ID_BANCO_CHEQUE { get; set; }
        public string BANCO { get; set; }
        public string CONTA_CHEQUE { get; set; }
        public Nullable<DateTime> DATA_COMPETENCIA { get; set; }
        public string NUM_CHEQUE { get; set; }
        public string AGENCIA { get; set; }
        public Nullable<DateTime> DATA_INCLUSAO { get; set; }
        public Nullable<int> ID_ENTRADA { get; set; }
        public Nullable<int> ID_CONVENIO { get; set; }
        public string CONVENIO { get; set; }
        public string TIPO_CARTAO { get; set; }
    }

    public class DRE_FLUXO_CAIXA
    {
        public string TIPO { get; set; }
        public string SUB_TIPO { get; set; }
        public string DESC_TIPO { get; set; }
        public string DESC_SUB_TIPO { get; set; }
        public string CATEGORIA { get; set; }
        public string SUBCATEGORIA { get; set; }
        public string MES_ANO { get; set; }
        public Nullable<decimal> VALOR { get; set; }
    }

    public class TOTAIS
    {
        public string TIPO { get; set; }
        public string SUB_TIPO { get; set; }
        public Nullable<decimal> TOTAL { get; set; }
    }        

    public class DocumentoFinanceiroController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/documentos
        [Route("api/documentos")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS()
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).ToList();
        }

        [Route("api/documentos/cliente/{idCliente}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOC(int idCliente)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.ID_CLIENTE == idCliente && d.DATA_QUITACAO == null && d.STATUS == "A" && d.DATA_VENCIMENTO <= DateTime.Now).ToList();
        }

        // GET api/documento/1
        [Route("api/documento/{idDocumento}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTO(int idDocumento)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.ID_DOCUMENTO == idDocumento).ToList();
        }

        [Route("api/documentos/cliente/{idCliente}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCs(int idCliente)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.ID_CLIENTE == idCliente && d.DATA_QUITACAO == null && d.STATUS == "A" && d.DATA_VENCIMENTO <= DateTime.Now).ToList();
        }
        // GET api/documentos/Login/PABLO
        [Route("api/documentos/Login/{login}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_LOGIN(string login)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.LOGIN == login).ToList();
        }

        // GET api/documento/pagar
        [Route("api/documentos/pagar")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_PAGAR()
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "D").ToList();
        }

        [Route("api/documentos/pagar/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_PAGAR(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                        int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "D" && DbFunctions.TruncateTime(d.DATA_INCLUSAO) >= DbFunctions.TruncateTime(dtInicial) &&
                                  DbFunctions.TruncateTime(d.DATA_INCLUSAO) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }

        [Route("api/documentos/pagar/vencimentos/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_PAGAR_VCTO(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                             int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "D" && DbFunctions.TruncateTime(d.DATA_VENCIMENTO) >= DbFunctions.TruncateTime(dtInicial) &&
                                  DbFunctions.TruncateTime(d.DATA_VENCIMENTO) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }

        [Route("api/documentos/pagar/competencia/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_PAGAR_COMP(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                             int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "D" && DbFunctions.TruncateTime(d.DATA_COMPETENCIA) >= DbFunctions.TruncateTime(dtInicial) &&
                                  DbFunctions.TruncateTime(d.DATA_COMPETENCIA) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }

        // GET api/documento/pagar/PABLO
        [Route("api/documentos/pagar/{login}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_PAGARLOGIN(string login)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "D" && d.LOGIN == login).ToList();
        }

        // GET api/documento/receber
        [Route("api/documentos/receber")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_RECEBER()
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "C").ToList();
        }

        [Route("api/documentos/receber/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_RECEBER(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                          int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "C" && DbFunctions.TruncateTime(d.DATA_INCLUSAO) >= DbFunctions.TruncateTime(dtInicial) &&
                                  DbFunctions.TruncateTime(d.DATA_INCLUSAO) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }

        [Route("api/documentos/receber/vencimentos/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_RECEBER_VCTO(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                               int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "C" && DbFunctions.TruncateTime(d.DATA_VENCIMENTO) >= DbFunctions.TruncateTime(dtInicial) &&
                                  DbFunctions.TruncateTime(d.DATA_VENCIMENTO) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }

        [Route("api/documentos/receber/competencia/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_RECEBER_COMP(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                               int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "C" && DbFunctions.TruncateTime(d.DATA_COMPETENCIA) >= DbFunctions.TruncateTime(dtInicial) &&
                                  DbFunctions.TruncateTime(d.DATA_COMPETENCIA) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }

        // GET api/documento/receber/PABLO
        [Route("api/documentos/receber/{login}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_RECEBERLOGIN(string login)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => d.TIPO == "C" && d.LOGIN == login).ToList();
        }

        // GET api/documento/receber/PABLO
        [Route("api/documentos/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                  int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        ID_CARTAO = d.ID_CARTAO,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => DbFunctions.TruncateTime(d.DATA_INCLUSAO) >= DbFunctions.TruncateTime(dtInicial) &&
                                  DbFunctions.TruncateTime(d.DATA_INCLUSAO) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA) ).ToList();
        }

        // GET api/documento/receber/PABLO
        [Route("api/documentos/vencimento/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_VENCIMENTO(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                            int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",                        
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_CARTAO = d.ID_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => DbFunctions.TruncateTime(d.DATA_VENCIMENTO) >= DbFunctions.TruncateTime(dtInicial) &&
                                   DbFunctions.TruncateTime(d.DATA_VENCIMENTO) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }

        // GET api/documento/receber
        [Route("api/documentos/quitacao/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_QUITACAO(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                            int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_CARTAO = d.ID_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => DbFunctions.TruncateTime(d.DATA_QUITACAO) >= DbFunctions.TruncateTime(dtInicial) &&
                                   DbFunctions.TruncateTime(d.DATA_QUITACAO) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }

        [Route("api/documentos/competencia/{dtInicial}/{dtFinal}/{idCentroCusto}/{idConta}/{idCategoria}/{idSubCategoria}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTOS_COMPETENCIA(DateTime? dtInicial, DateTime? dtFinal, int idCentroCusto,
                                                                            int idConta, int idCategoria, int idSubCategoria)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    join conv in db.CONVENIOs on d.ID_CONVENIO equals conv.ID_CONVENIO into _conv
                    from conv in _conv.DefaultIfEmpty()
                    join ct in db.CARTAOs on d.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_CARTAO = d.ID_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA,
                        ID_CONVENIO = d.ID_CONVENIO,
                        CONVENIO = conv.DESCRICAO,
                        TIPO_CARTAO = (ct.TIPO == "c" ? "CRÉDITO" : ct.TIPO == "D" ? "DÉBITO" : "")
                    }).Where(d => DbFunctions.TruncateTime(d.DATA_COMPETENCIA) >= DbFunctions.TruncateTime(dtInicial) &&
                                   DbFunctions.TruncateTime(d.DATA_COMPETENCIA) <= DbFunctions.TruncateTime(dtFinal) &&
                                  d.ID_CENTRO_CUSTO == (idCentroCusto != 0 ? idCentroCusto : d.ID_CENTRO_CUSTO) &&
                                  d.ID_CONTA == (idConta != 0 ? idConta : d.ID_CONTA) &&
                                  d.ID_CATEGORIA == (idCategoria != 0 ? idCategoria : d.ID_CATEGORIA) &&
                                  d.ID_SUB_CATEGORIA == (idSubCategoria != 0 ? idSubCategoria : d.ID_SUB_CATEGORIA)).ToList();
        }
                
        [Route("api/documento/fluxocaixaDre/{rel}/{visao}/{tipo}/{dtIni}/{dtFim}/{idConta}/{idCentroCusto}/{idUnidade}/{situacao}")]
        public DataTable GetDreFluxoCaixa(string rel, string visao, string tipo, DateTime dtIni, DateTime dtFim, int idConta, int idCentroCusto, int idUnidade, string situacao)
        {
            DataSet sampleDataSet = new DataSet();
            sampleDataSet.Locale = System.Globalization.CultureInfo.InvariantCulture;
            DataTable sampleDataTable = new DataTable("FluxoCaixa");
            PreencheColunas(ref sampleDataTable, tipo, dtIni, dtFim);
            if (rel == "F")
                PreencheLinhasFluxoCaixa(ref sampleDataTable, rel, visao, tipo, dtIni, dtFim, idConta, idCentroCusto, idUnidade, situacao);            
            else
                PreencheLinhasDre(ref sampleDataTable, rel, visao, tipo, dtIni, dtFim, idConta, idCentroCusto, idUnidade, situacao);
            return sampleDataTable;
        }

        public IEnumerable<DRE_FLUXO_CAIXA> GetFluxoCaixa(string tipo, DateTime dtIni, DateTime dtFim, int idConta, int idCentroCusto, int idUnidade, string situacao)
        {
            string datainicial = dtIni.ToString("yyyy-MM-dd");
            string datafinal = dtFim.ToString("yyyy-MM-dd");    
            StringBuilder sbSql = new StringBuilder();

            List<DRE_FLUXO_CAIXA> resultado = new List<DRE_FLUXO_CAIXA>();
            IEnumerable<DRE_FLUXO_CAIXA> dadosbd;

            sbSql.Append(@"select C.TIPO, ISNULL(C.SUB_TIPO, 'Z') SUB_TIPO, 
                        CASE WHEN C.TIPO = 'B' THEN '- SAÍDA' 
                            WHEN C.TIPO = 'A' THEN '+ ENTRADA'  
                            WHEN C.TIPO = 'C' THEN '- INVESTIMENTO'  
                            WHEN C.TIPO = 'D' THEN  '- DISTRIBUIÇÃO DE LUCRO' END AS DESC_TIPO, 
                        CASE WHEN C.SUB_TIPO = 'A' THEN 'CUSTO DIRETO'   
                            WHEN C.SUB_TIPO = 'B' THEN 'CUSTOS E SERVIÇOS OPERACIONAIS'   
                            WHEN C.SUB_TIPO = 'C' THEN 'CUSTOS E SERVIÇOS NÃO OPERACIONAIS'   
                            WHEN C.SUB_TIPO = 'D' THEN 'DGA - DESPESAS GERAIS E ADMINISTRATIVAS'   
                            WHEN C.SUB_TIPO = 'E' THEN 'RECEITAS OPERACIONAIS'  
                            WHEN C.SUB_TIPO = 'F' THEN 'RECEITAS NÃO OPERACIONAIS'
                            WHEN C.SUB_TIPO = 'J' THEN 'AJUSTE DOCUMENTOS'
                        ELSE 'DOCUMENTO NÃO CATEGORIZADO' END AS DESC_SUB_TIPO,
                               C.DESCRICAO AS CATEGORIA, SC.DESCRICAO AS SUBCATEGORIA, ");

            if (tipo == "D")
                sbSql.Append(" LEFT(CONVERT(VARCHAR, cast(d.DATA_VENCIMENTO as date), 103), 10)  AS MES_ANO, ");
            else
                sbSql.Append(" (CAST(month(d.DATA_VENCIMENTO) AS VARCHAR) + '/' + CAST(YEAR(d.DATA_VENCIMENTO) AS VARCHAR)) AS MES_ANO, ");

            sbSql.Append(@" cast(case when C.TIPO = 'A' then  sum(d.VALOR) else sum(d.VALOR) * -1 end  AS decimal(15,2)) VALOR
                        from DOCUMENTO_FINANCEIRO D 
                        INNER JOIN SUB_CATEGORIA SC on SC.ID_SUB_CATEGORIA = D.ID_SUB_CATEGORIA 
                        INNER JOIN CATEGORIA C ON C.ID_CATEGORIA = D.ID_CATEGORIA 
                        WHERE ");

            sbSql.Append(string.Format(@" cast(d.DATA_VENCIMENTO as date) >= cast('{0}' as date) 
                                        AND cast(d.DATA_VENCIMENTO as date) <= cast('{1}' as date) ", datainicial, datafinal));

            if (idConta > 0)
                sbSql.Append(string.Format("AND d.ID_CONTA = {0}", idConta));
            if (idCentroCusto > 0)
                sbSql.Append(string.Format(" AND d.ID_CENTRO_CUSTO = {0} ", idCentroCusto));
            if (idUnidade > 0)
                sbSql.Append(string.Format(" AND d.ID_CONTA in (select id_conta from CONTA CC  where cc.ID_UNIDADE = {0})", idUnidade));
            if (situacao != "0")
            {
                if (situacao == "1")
                    sbSql.Append(" AND D.SALDO > 0");
                else
                    sbSql.Append(" AND D.SALDO = 0");
            }

            if (tipo == "D")
                sbSql.Append(@" group by C.TIPO, C.SUB_TIPO, C.DESCRICAO, SC.DESCRICAO, cast(d.DATA_VENCIMENTO as date) 
                                order by C.TIPO, ISNULL(C.SUB_TIPO, 'Z'), C.DESCRICAO, SC.DESCRICAO, cast(d.DATA_VENCIMENTO as date)");
            else
                sbSql.Append(@" group by C.TIPO, C.SUB_TIPO, C.DESCRICAO, SC.DESCRICAO, month(d.DATA_VENCIMENTO), YEAR(d.DATA_VENCIMENTO) 
                                order by C.TIPO, ISNULL(C.SUB_TIPO, 'Z'), C.DESCRICAO, SC.DESCRICAO, month(d.DATA_VENCIMENTO), YEAR(d.DATA_VENCIMENTO)");

            dadosbd = db.Database.SqlQuery<DRE_FLUXO_CAIXA>(sbSql.ToString()).ToList();
            resultado = (List<DRE_FLUXO_CAIXA>)dadosbd;           

            return resultado;
        }

        public IEnumerable<DRE_FLUXO_CAIXA> GetDre(string tipo, DateTime dtIni, DateTime dtFim, int idConta, int idCentroCusto, int idUnidade, string situacao)
        {
            string datainicial = dtIni.ToString("yyyy-MM-dd");
            string datafinal = dtFim.ToString("yyyy-MM-dd");
            StringBuilder sbSql = new StringBuilder();

            List<DRE_FLUXO_CAIXA> resultado = new List<DRE_FLUXO_CAIXA>();
            IEnumerable<DRE_FLUXO_CAIXA> dadosbd;

            sbSql.Append(@"select C.TIPO, ISNULL(C.SUB_TIPO, 'Z') SUB_TIPO,
                        CASE WHEN C.TIPO = 'B' THEN '- SAÍDA'
                            WHEN C.TIPO = 'A' THEN '+ ENTRADA'
                            WHEN C.TIPO = 'C' THEN '- INVESTIMENTO'
                            WHEN C.TIPO = 'D' THEN  '- DISTRIBUIÇÃO DE LUCRO' END AS DESC_TIPO,
                        CASE WHEN C.SUB_TIPO = 'A' THEN 'CUSTO DIRETO'
                            WHEN C.SUB_TIPO = 'B' THEN 'CUSTOS E SERVIÇOS OPERACIONAIS'
                            WHEN C.SUB_TIPO = 'C' THEN 'CUSTOS E SERVIÇOS NÃO OPERACIONAIS'
                            WHEN C.SUB_TIPO = 'D' THEN 'DGA - DESPESAS GERAIS E ADMINISTRATIVAS'
                            WHEN C.SUB_TIPO = 'E' THEN 'RECEITAS OPERACIONAIS'
                            WHEN C.SUB_TIPO = 'F' THEN 'RECEITAS NÃO OPERACIONAIS'
                            WHEN C.SUB_TIPO = 'J' THEN 'AJUSTE DOCUMENTOS'
                        ELSE 'DOCUMENTO NÃO CATEGORIZADO' END AS DESC_SUB_TIPO,
                               C.DESCRICAO AS CATEGORIA, SC.DESCRICAO AS SUBCATEGORIA, ");

            if (tipo == "D")
                sbSql.Append(" LEFT(CONVERT(VARCHAR, cast(d.DATA_COMPETENCIA as date), 103), 10)  AS MES_ANO, ");
            else
                sbSql.Append(" (CAST(month(d.DATA_COMPETENCIA) AS VARCHAR) + '/' + CAST(YEAR(d.DATA_COMPETENCIA) AS VARCHAR)) AS MES_ANO, ");

            sbSql.Append(@" cast(case when C.TIPO = 'A' then  sum(d.VALOR) else sum(d.VALOR) * -1 end  AS decimal(15,2)) VALOR
                        from DOCUMENTO_FINANCEIRO D 
                        INNER JOIN SUB_CATEGORIA SC on SC.ID_SUB_CATEGORIA = D.ID_SUB_CATEGORIA 
                        INNER JOIN CATEGORIA C ON C.ID_CATEGORIA = D.ID_CATEGORIA 
                        WHERE ");

            sbSql.Append(string.Format(@" cast(d.DATA_COMPETENCIA as date) >= cast('{0}' as date) 
                                        AND cast(d.DATA_COMPETENCIA as date) <= cast('{1}' as date) ", datainicial, datafinal));

            if (idConta > 0)
                sbSql.Append(string.Format("AND d.ID_CONTA = {0}", idConta));
            if (idCentroCusto > 0)
                sbSql.Append(string.Format(" AND d.ID_CENTRO_CUSTO = {0} ", idCentroCusto));
            if (idUnidade > 0)
                sbSql.Append(string.Format(" AND d.ID_CONTA in (select id_conta from CONTA CC  where cc.ID_UNIDADE = {0})", idUnidade));
            if (situacao != "0")
            {
                if (situacao == "1")
                    sbSql.Append(" AND D.SALDO > 0");
                else
                    sbSql.Append(" AND D.SALDO = 0");
            }

            if (tipo == "D")
                sbSql.Append(@" group by C.TIPO, C.SUB_TIPO, C.DESCRICAO, SC.DESCRICAO, cast(d.DATA_COMPETENCIA as date) 
                                order by C.TIPO, ISNULL(C.SUB_TIPO, 'Z'), C.DESCRICAO, SC.DESCRICAO, cast(d.DATA_COMPETENCIA as date)");
            else
                sbSql.Append(@" group by C.TIPO, C.SUB_TIPO, C.DESCRICAO, SC.DESCRICAO, month(d.DATA_COMPETENCIA), YEAR(d.DATA_COMPETENCIA) 
                                order by C.TIPO, ISNULL(C.SUB_TIPO, 'Z'), C.DESCRICAO, SC.DESCRICAO, month(d.DATA_COMPETENCIA), YEAR(d.DATA_COMPETENCIA)");

            dadosbd = db.Database.SqlQuery<DRE_FLUXO_CAIXA>(sbSql.ToString()).ToList();
            resultado = (List<DRE_FLUXO_CAIXA>)dadosbd;
            return resultado;
        }

        public void PreencheColunas(ref DataTable sampleDataTable, string tipoPeriodo, DateTime dtIni, DateTime dtFim)
        {
            sampleDataTable.Columns.Add("ROWID", typeof(int));
            sampleDataTable.Columns.Add("TOTAL", typeof(decimal));
            sampleDataTable.Columns.Add("DESCRICAO", typeof(string));
            string meses = string.Empty;
            DateTime dtAux = dtIni;
            string mesAno = string.Empty;
            while (dtAux <= dtFim)
            {
                if (tipoPeriodo == "M")
                {
                    if (mesAno != dtAux.Month.ToString() + "/" + dtAux.Year.ToString())
                    {
                        mesAno = dtAux.Month.ToString() + "/" + dtAux.Year.ToString();
                        sampleDataTable.Columns.Add(mesAno, typeof(decimal));
                    }
                }
                else
                {
                    sampleDataTable.Columns.Add(dtAux.Date.ToString("dd/MM/yyyy"), typeof(decimal));
                }
                dtAux = dtAux.AddDays(1);
            }
        }
        public void PreencheLinhasFluxoCaixa(ref DataTable sampleDataTable, string rel, string visao, string tipo, DateTime dtIni, DateTime dtFim, int idConta, int idCentroCusto, int idUnidade, string situacao)
        {
            IEnumerable<DRE_FLUXO_CAIXA> dados = null;
            dados = GetFluxoCaixa(tipo, dtIni, dtFim, idConta, idCentroCusto, idUnidade, situacao);

            int rowId = 0;
            StringBuilder sbCategoria = new StringBuilder();
            StringBuilder sbCompareCategoria = new StringBuilder();
            StringBuilder sbSubCategoria = new StringBuilder();
            StringBuilder sbCompareSubCategoria = new StringBuilder();
            foreach (DRE_FLUXO_CAIXA reg in dados)
            {
                sbCategoria.Clear();
                sbCategoria.Append(reg.DESC_SUB_TIPO);
                sbCategoria.Append("|");
                sbCategoria.Append(reg.CATEGORIA);

                sbSubCategoria.Clear();
                sbSubCategoria.Append(reg.CATEGORIA);
                sbSubCategoria.Append("-");
                sbSubCategoria.Append(reg.SUBCATEGORIA);

                if (!DescricaoExistente(reg.DESC_TIPO, sampleDataTable))
                {                    
                    if(sampleDataTable.Rows.Count > 0)
                    {
                        rowId++;
                        preencheLinhaVazia(ref sampleDataTable, rowId);
                    }
                    rowId++;
                    preencheLinhaPorTipo(ref sampleDataTable, rowId, reg, dados, string.Empty);                                        
                }

                if (!DescricaoExistente(reg.DESC_SUB_TIPO, sampleDataTable))
                {
                    rowId++;
                    preencheLinhaPorSubTipo(ref sampleDataTable, rowId, reg, dados, string.Empty);                    
                }
                if (visao == "A")
                {
                    if (sbSubCategoria.ToString() != sbCompareSubCategoria.ToString())
                    {
                        sbCompareSubCategoria.Clear();
                        sbCompareSubCategoria.Append(reg.CATEGORIA);
                        sbCompareSubCategoria.Append("-");
                        sbCompareSubCategoria.Append(reg.SUBCATEGORIA);

                        rowId++;
                        preencheLinhaPorSubCategoria(ref sampleDataTable, rowId, reg, dados, string.Empty);
                    }
                }
            }
            rowId++;
            preencheLinhaVazia(ref sampleDataTable, rowId);
            rowId++;
            preencheLinhaTotal(ref sampleDataTable, rowId, dados, "F");
            rowId++;
            preencheLinhaPercentual(ref sampleDataTable, rowId, dados, "F");
        }
        public void PreencheLinhasDre(ref DataTable sampleDataTable, string rel, string visao, string tipo, DateTime dtIni, DateTime dtFim, int idConta, int idCentroCusto, int idUnidade, string situacao)
        {
            IEnumerable<DRE_FLUXO_CAIXA> dados = null;
            dados = GetDre(tipo, dtIni, dtFim, idConta, idCentroCusto, idUnidade, situacao);
            int rowId = 0;
            PreencheLinhasDreTipo(dados, ref sampleDataTable, rel, visao, "A", ref rowId);
            rowId++;
            PreencheLinhasDreTipo(dados, ref sampleDataTable, rel, visao, "B", ref rowId);
            rowId++;
            preencheLinhaVazia(ref sampleDataTable, rowId);
            rowId++;
            preencheLinhaTotal(ref sampleDataTable, rowId, dados, rel);
            rowId++;
            preencheLinhaPercentual(ref sampleDataTable, rowId, dados, rel);
            rowId++;            
            PreencheLinhasDreTipo(dados, ref sampleDataTable, rel, visao, "C", ref rowId);
            rowId++;
            PreencheLinhasDreTipo(dados, ref sampleDataTable, rel, visao, "D", ref rowId);
        }
        public void PreencheLinhasDreTipo(IEnumerable<DRE_FLUXO_CAIXA> dados, ref DataTable sampleDataTable, string rel, string visao, string tipo, ref int rowId)
        {      
            StringBuilder sbCategoria = new StringBuilder();
            StringBuilder sbCompareCategoria = new StringBuilder();
            StringBuilder sbSubCategoria = new StringBuilder();
            StringBuilder sbCompareSubCategoria = new StringBuilder();            
            foreach (DRE_FLUXO_CAIXA reg in dados.Where(x => x.TIPO == tipo))
            {
                sbCategoria.Clear();
                sbCategoria.Append(reg.DESC_SUB_TIPO);
                sbCategoria.Append("|");
                sbCategoria.Append(reg.CATEGORIA);

                sbSubCategoria.Clear();
                sbSubCategoria.Append(reg.CATEGORIA);
                sbSubCategoria.Append("-");
                sbSubCategoria.Append(reg.SUBCATEGORIA);

                if (!DescricaoExistente(reg.DESC_TIPO, sampleDataTable))
                {
                    if (sampleDataTable.Rows.Count > 0)
                    {
                        rowId++;
                        preencheLinhaVazia(ref sampleDataTable, rowId);
                    }
                    rowId++;
                    preencheLinhaPorTipo(ref sampleDataTable, rowId, reg, dados, tipo);
                }

                if (!DescricaoExistente(reg.DESC_SUB_TIPO, sampleDataTable))
                {
                    rowId++;
                    preencheLinhaPorSubTipo(ref sampleDataTable, rowId, reg, dados, tipo);
                }
                if (visao == "A")
                {
                    if (sbSubCategoria.ToString() != sbCompareSubCategoria.ToString())
                    {
                        sbCompareSubCategoria.Clear();
                        sbCompareSubCategoria.Append(reg.CATEGORIA);
                        sbCompareSubCategoria.Append("-");
                        sbCompareSubCategoria.Append(reg.SUBCATEGORIA);

                        rowId++;
                        preencheLinhaPorSubCategoria(ref sampleDataTable, rowId, reg, dados, tipo);
                    }
                }
            }            
        }
        public void preencheLinhaPorTipo(ref DataTable sampleDataTable, int rowId, DRE_FLUXO_CAIXA registro, IEnumerable<DRE_FLUXO_CAIXA> dados, string tipo)
        {            
            DataRow sampleDataRow;
            sampleDataRow = sampleDataTable.NewRow();
            sampleDataRow["ROWID"] = rowId;
            sampleDataRow["DESCRICAO"] = registro.DESC_TIPO;
            string stTipo = (tipo == string.Empty) ? registro.TIPO : tipo;
            IEnumerable<DRE_FLUXO_CAIXA> dadosAux = dados.Where(a => a.TIPO == stTipo);
            decimal? valor = 0;            
            if (dadosAux != null)
            {
                valor = dadosAux.Sum(a => a.VALOR);
            }
            sampleDataRow["TOTAL"] = valor;
            foreach (DataColumn col in sampleDataTable.Columns)
            {
                if (col.ColumnName != "DESCRICAO" && col.ColumnName != "ROWID" && col.ColumnName != "TOTAL")
                {
                    valor = 0;
                    IEnumerable<DRE_FLUXO_CAIXA> dadosTotal = dadosAux.Where(a => a.MES_ANO == col.ColumnName);
                    if (dadosTotal != null)
                    {
                        valor = dadosTotal.Sum(a => a.VALOR);
                    }
                    sampleDataRow[col.ColumnName] = valor;
                }
            }
            sampleDataTable.Rows.Add(sampleDataRow);
        }
        public void preencheLinhaPorSubTipo(ref DataTable sampleDataTable, int rowId, DRE_FLUXO_CAIXA registro, IEnumerable<DRE_FLUXO_CAIXA> dados, string tipo)
        {
            DataRow sampleDataRow;
            sampleDataRow = sampleDataTable.NewRow();
            sampleDataRow["ROWID"] = rowId;
            sampleDataRow["DESCRICAO"] = registro.DESC_SUB_TIPO;
            string stTipo = (tipo == string.Empty) ? registro.TIPO : tipo;
            IEnumerable<DRE_FLUXO_CAIXA> dadosAux = dados.Where(a => a.TIPO == stTipo && a.SUB_TIPO == registro.SUB_TIPO);
            decimal? valor = 0;
            if (dadosAux != null)
            {
                valor = dadosAux.Sum(a => a.VALOR);
            }
            sampleDataRow["TOTAL"] = valor;
            foreach (DataColumn col in sampleDataTable.Columns)
            {
                if (col.ColumnName != "DESCRICAO" && col.ColumnName != "ROWID" && col.ColumnName != "TOTAL")
                {
                    valor = 0;
                    IEnumerable<DRE_FLUXO_CAIXA> dadosTotal = dadosAux.Where(a => a.MES_ANO == col.ColumnName);
                    if (dadosTotal != null)
                    {
                        valor = dadosTotal.Sum(a => a.VALOR);
                    }
                    sampleDataRow[col.ColumnName] = valor;
                }
            }
            sampleDataTable.Rows.Add(sampleDataRow);
        }        
        public void preencheLinhaPorSubCategoria(ref DataTable sampleDataTable, int rowId, DRE_FLUXO_CAIXA registro, IEnumerable<DRE_FLUXO_CAIXA> dados, string tipo)
        {
            DataRow sampleDataRow;
            sampleDataRow = sampleDataTable.NewRow();
            sampleDataRow["ROWID"] = rowId;
            sampleDataRow["DESCRICAO"] = string.Format("{0} >> {1}", registro.CATEGORIA, registro.SUBCATEGORIA);
            string stTipo = (tipo == string.Empty) ? registro.TIPO : tipo;
            IEnumerable<DRE_FLUXO_CAIXA> dadosAux = dados.Where(a => a.TIPO == stTipo && a.SUB_TIPO == registro.SUB_TIPO && a.CATEGORIA == registro.CATEGORIA && a.SUBCATEGORIA == registro.SUBCATEGORIA);
            decimal? valor = 0;
            if (dadosAux != null)
            {
                valor = dadosAux.Sum(a => a.VALOR);
            }
            sampleDataRow["TOTAL"] = valor;
            foreach (DataColumn col in sampleDataTable.Columns)
            {
                if (col.ColumnName != "DESCRICAO" && col.ColumnName != "ROWID" && col.ColumnName != "TOTAL")
                {
                    valor = 0;
                    IEnumerable<DRE_FLUXO_CAIXA> dadosTotal = dadosAux.Where(a => a.MES_ANO == col.ColumnName);
                    if (dadosTotal != null)
                    {
                        valor = dadosTotal.Sum(a => a.VALOR);
                    }
                    sampleDataRow[col.ColumnName] = valor;
                }
            }
            sampleDataTable.Rows.Add(sampleDataRow);
        }
        public void preencheLinhaTotal(ref DataTable sampleDataTable, int rowId, IEnumerable<DRE_FLUXO_CAIXA> dados, string rel)
        {
            DataRow sampleDataRow;
            sampleDataRow = sampleDataTable.NewRow();
            sampleDataRow["ROWID"] = rowId;
            sampleDataRow["DESCRICAO"] = "RESULTADO OPERACIONAL";

            IEnumerable<DRE_FLUXO_CAIXA> dadosAux = dados;
            if(rel != "F")
                dadosAux = dados.Where(x => x.TIPO == "A" || x.TIPO == "B");

            decimal? valor = 0;
            if (dadosAux != null)
            {
                valor = dadosAux.Sum(a => a.VALOR);
            }
            sampleDataRow["TOTAL"] = valor;

            dadosAux = dados;
            if (rel != "F")
                dadosAux = dados.Where(x => x.TIPO == "A" || x.TIPO == "B");

            foreach (DataColumn col in sampleDataTable.Columns)
            {
                if (col.ColumnName != "DESCRICAO" && col.ColumnName != "ROWID" && col.ColumnName != "TOTAL")
                {
                    valor = 0;
                    IEnumerable<DRE_FLUXO_CAIXA> dadosTotal = dadosAux.Where(a => a.MES_ANO == col.ColumnName);
                    if (dadosTotal != null)
                    {
                        valor = dadosTotal.Sum(a => a.VALOR);
                    }
                    sampleDataRow[col.ColumnName] = valor;
                }
            }
            sampleDataTable.Rows.Add(sampleDataRow);
        }
        public void preencheLinhaPercentual(ref DataTable sampleDataTable, int rowId, IEnumerable<DRE_FLUXO_CAIXA> dados, string rel)
        {
            DataRow sampleDataRow;
            sampleDataRow = sampleDataTable.NewRow();
            sampleDataRow["ROWID"] = rowId;
            sampleDataRow["DESCRICAO"] = "%";

            IEnumerable<DRE_FLUXO_CAIXA> dadosAux = dados.Where(a => a.TIPO == "A");
            decimal? valorTotalEntrada = 0;
            if (dadosAux != null)
            {
                valorTotalEntrada = dadosAux.Sum(a => a.VALOR);
            }

            dadosAux = dados;
            if (rel != "F")
                dadosAux = dados.Where(x => x.TIPO == "A" || x.TIPO == "B");

            decimal? valorTotal = 0;
            if (dadosAux != null)
            {
                valorTotal = dadosAux.Sum(a => a.VALOR);
            }
            sampleDataRow["TOTAL"] = 0;
            if (valorTotal.Value > 0 && valorTotalEntrada.Value > 0)
                sampleDataRow["TOTAL"] = (valorTotal.Value * 100)/valorTotalEntrada.Value;


            IEnumerable<DRE_FLUXO_CAIXA> dadosDreFluxoCaixa = dados;
            if (rel != "F")
                dadosDreFluxoCaixa = dados.Where(x => x.TIPO == "A" || x.TIPO == "B");

            foreach (DataColumn col in sampleDataTable.Columns)
            {
                if (col.ColumnName != "DESCRICAO" && col.ColumnName != "ROWID" && col.ColumnName != "TOTAL")
                {
                    valorTotal = 0;
                    valorTotalEntrada = 0;
                    dadosAux = dados.Where(a => a.TIPO == "A" && a.MES_ANO == col.ColumnName);                    
                    if (dadosAux != null)
                    {
                        valorTotalEntrada = dadosAux.Sum(a => a.VALOR);
                    }

                    IEnumerable<DRE_FLUXO_CAIXA> dadosTotal = dadosDreFluxoCaixa.Where(a => a.MES_ANO == col.ColumnName);
                    if (dadosTotal != null)
                    {
                        valorTotal = dadosTotal.Sum(a => a.VALOR);
                    }
                    sampleDataRow[col.ColumnName] = 0;
                    if (valorTotal.Value > 0 && valorTotalEntrada.Value > 0)
                        sampleDataRow[col.ColumnName] = (valorTotal.Value * 100) / valorTotalEntrada.Value;
                }
            }
            sampleDataTable.Rows.Add(sampleDataRow);
        }
        public void preencheLinhaVazia(ref DataTable sampleDataTable, int rowId)
        {
            DataRow sampleDataRow;
            sampleDataRow = sampleDataTable.NewRow();
            sampleDataRow["ROWID"] = rowId;
            sampleDataRow["DESCRICAO"] = null;
            sampleDataRow["TOTAL"] = DBNull.Value;
            foreach (DataColumn col in sampleDataTable.Columns)
            {
                if (col.ColumnName != "DESCRICAO" && col.ColumnName != "ROWID" && col.ColumnName != "TOTAL")
                {
                    sampleDataRow[col.ColumnName] = DBNull.Value;
                }
            }
            sampleDataTable.Rows.Add(sampleDataRow);
        }
        public bool DescricaoExistente(string descricao, DataTable sampleDataTable)
        {
            //return sampleDataTable.Select(string.Format("DESCRICAO={0}{1}{2}",descricao)) != null;
            foreach (DataRow dt in sampleDataTable.Rows)
            {
                if (dt["DESCRICAO"].ToString() == descricao)
                {
                    return true;
                }
            }
            return false;
        }

        // GET api/documento/1
        [Route("api/documento/entrada_estoque/{idEntrada}")]
        public IEnumerable<DOCUMENTO_FINANCEIRODTO> GetDOCUMENTO_ENTRADA_ESTOQUE(int idEntrada)
        {
            return (from d in db.DOCUMENTO_FINANCEIRO
                    join c in db.CLIENTES on d.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    join f in db.FORNECEDORs on d.ID_FORNECEDOR equals f.ID_FORNECEDOR into _f
                    from f in _f.DefaultIfEmpty()
                    join co in db.CONTAs on d.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join u in db.UNIDADEs on co.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on d.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on d.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join ca in db.CATEGORIAs on d.ID_CATEGORIA equals ca.ID_CATEGORIA into _ca
                    from ca in _ca.DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on d.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on d.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    join b in db.BANCOes on d.ID_BANCO_CHEQUE equals b.ID_BANCO into _b
                    from b in _b.DefaultIfEmpty()
                    join cx in db.CAIXA_ITEM on d.ID_ITEM_CAIXA equals cx.ID_ITEM_CAIXA into _cx
                    from cx in _cx.DefaultIfEmpty()
                    join vi in db.VENDA_PGTOS on cx.ID_ITEM_PGTO_VENDA equals vi.ID_ITEM_PGTO_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join crt in db.CARTAOs on vi.ID_CARTAO equals crt.ID_CARTAO into _crt
                    from crt in _crt.DefaultIfEmpty()
                    select new DOCUMENTO_FINANCEIRODTO()
                    {
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        TIPO = d.TIPO,
                        DATA_ATUALIZACAO = d.DATA_ATUALIZACAO,
                        LOGIN = d.LOGIN,
                        VALOR = d.VALOR,
                        DATA_VENCIMENTO = d.DATA_VENCIMENTO,
                        NUM_DOCUMENTO = d.NUM_DOCUMENTO,
                        ID_CONTA = d.ID_CONTA,
                        ID_CENTRO_CUSTO = d.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = d.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = d.ID_SUB_CATEGORIA,
                        STATUS = d.STATUS,
                        ID_CLIENTE = d.ID_CLIENTE,
                        ID_ITEM_CAIXA = d.ID_ITEM_CAIXA,
                        ID_FORNECEDOR = d.ID_FORNECEDOR,
                        SALDO = d.SALDO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        DESC_STATUS = d.STATUS == "N" ? "LIBERAR" :
                                      d.STATUS == "A" ? "ABERTO" :
                                      d.STATUS == "Q" ? "QUITADO" :
                                      d.STATUS == "V" ? "VENCIDO" : "",
                        CLIENTE = c.NOME,
                        CATEGORIA = ca.DESCRICAO,
                        DESC_TIPO = d.TIPO == "D" ? "PAGAR" :
                                    d.TIPO == "C" ? "RECEBER" : "",
                        FORNECEDOR = f.NOME_FANTASIA,
                        ID_EQUIPAMENTO = d.ID_EQUIPAMENTO,
                        EQUIPAMENTO = e.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        TIPO_PGTO = d.TIPO_PGTO,
                        NUM_PARCELA = d.NUM_PARCELA,
                        DESC_TIPO_PGTO = d.TIPO_PGTO == 0 ? "DINHEIRO" :
                                         d.TIPO_PGTO == 1 ? "CHEQUE" :
                                         d.TIPO_PGTO == 2 ? "CARTÃO" :
                                         d.TIPO_PGTO == 3 ? "BOLETO" :
                                         d.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        BANDEIRA_CARTAO = crt.BANDEIRA,
                        DATA_QUITACAO = d.DATA_QUITACAO,
                        OBS = d.OBS,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_PROFISSIONAL = d.ID_PROFISSIONAL,
                        PROFISSIONAL = p.NOME,
                        CV_CARTAO = d.CV_CARTAO,
                        ID_BANCO_CHEQUE = d.ID_BANCO_CHEQUE,
                        BANCO = b.NOME,
                        CONTA_CHEQUE = d.CONTA_CHEQUE,
                        DATA_COMPETENCIA = d.DATA_COMPETENCIA,
                        AGENCIA = d.AGENCIA,
                        NUM_CHEQUE = d.NUM_CHEQUE,
                        DATA_INCLUSAO = d.DATA_INCLUSAO,
                        ID_ENTRADA = d.ID_ENTRADA
                    }).Where(d => d.ID_ENTRADA == idEntrada).ToList();
        }

        // PUT: api/DocumentoFinanceiro/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDOCUMENTO_FINANCEIRO(int id, DOCUMENTO_FINANCEIRO dOCUMENTO_FINANCEIRO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dOCUMENTO_FINANCEIRO.ID_DOCUMENTO)
            {
                return BadRequest();
            }

            db.Entry(dOCUMENTO_FINANCEIRO).State = EntityState.Modified;

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DOCUMENTO_FINANCEIROExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DocumentoFinanceiro
        [ResponseType(typeof(DOCUMENTO_FINANCEIRO))]
        public IHttpActionResult PostDOCUMENTO_FINANCEIRO(DOCUMENTO_FINANCEIRO dOCUMENTO_FINANCEIRO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_DOCUMENTO;").First();

            dOCUMENTO_FINANCEIRO.ID_DOCUMENTO = NextValue;

            db.DOCUMENTO_FINANCEIRO.Add(dOCUMENTO_FINANCEIRO);

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DOCUMENTO_FINANCEIROExists(dOCUMENTO_FINANCEIRO.ID_DOCUMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = dOCUMENTO_FINANCEIRO.ID_DOCUMENTO }, dOCUMENTO_FINANCEIRO);
        }

        // DELETE: api/DocumentoFinanceiro/5
        [ResponseType(typeof(DOCUMENTO_FINANCEIRO))]
        public IHttpActionResult DeleteDOCUMENTO_FINANCEIRO(int id)
        {
            DOCUMENTO_FINANCEIRO dOCUMENTO_FINANCEIRO = db.DOCUMENTO_FINANCEIRO.Find(id); //  .Where(c=>c.ID_DOCUMENTO == id).FirstOrDefault();
            if (dOCUMENTO_FINANCEIRO == null)
            {
                return NotFound();
            }

            db.DOCUMENTO_FINANCEIRO.Remove(dOCUMENTO_FINANCEIRO);
            //db.BulkSaveChanges();
            db.SaveChanges();

            return Ok(dOCUMENTO_FINANCEIRO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DOCUMENTO_FINANCEIROExists(int id)
        {
            return db.DOCUMENTO_FINANCEIRO.Count(e => e.ID_DOCUMENTO == id) > 0;
        }
    }
}