﻿using Microsoft.EntityFrameworkCore;
using ReporteIncidentes.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace ReporteIncidentes.DAL
{
    public class IncidenciasDAL
    {
        private readonly Contexto _contexto;
        /// <summary>
		/// Constructor de la clase
		/// </summary>
		/// <param name="contexto"></param>
        public IncidenciasDAL(Contexto contexto)
        {
            _contexto = contexto;
        }
		/// <summary>
		/// Método para insertar las incidencias
		/// </summary>
		/// <param name="incidencias"></param>
		/// <returns></returns>
		public Respuesta<bool> InsertarIncidencias(Incidencias incidencias)
		{
			Respuesta<bool> respuesta = new Respuesta<bool>();
			using (TransactionScope transaccion = new TransactionScope())
			{
				try
				{
					string SQL = @"EXEC Pa_InsertarIncidencia @IdUsuario,@Categoria,@Empresa,@Provincia,
									@Canton,@Distrito,@DireccionExacta,@Latitud,@Longitud,@RutaImagen1,
									@RutaImagen2,@RutaImagen3,@RutaImagen4,@DetalleIncidencia";
					_contexto.Database.ExecuteSqlCommand(SQL,
						new SqlParameter("@IdUsuario", incidencias.IdUsuario),
						new SqlParameter("@Categoria", incidencias.Categoria),
						new SqlParameter("@Empresa", incidencias.Empresa),
						new SqlParameter("@Provincia", incidencias.Provincia),
						new SqlParameter("@Canton", incidencias.Canton),
						new SqlParameter("@Distrito", incidencias.Distrito),
						new SqlParameter("@DireccionExacta", incidencias.DireccionExacta),
						new SqlParameter("@Latitud", incidencias.Latitud),
						new SqlParameter("@Longitud", incidencias.Longitud),
						new SqlParameter("@RutaImagen1", incidencias.RutaImagen1),
						new SqlParameter("@RutaImagen2", incidencias.RutaImagen2),
						new SqlParameter("@RutaImagen3", incidencias.RutaImagen3),
						new SqlParameter("@RutaImagen4", incidencias.RutaImagen4),
						new SqlParameter("@DetalleIncidencia", incidencias.DetalleIncidencia),
				   _contexto.SaveChanges());
					transaccion.Complete();
					respuesta.HayError = false;
					respuesta.ObjetoRespuesta = true;
				}
				catch (Exception ex)
				{
					transaccion.Dispose();
					respuesta.HayError = true;
					respuesta.MensajeError = ex.Message;
					respuesta.ObjetoRespuesta = false;
				}
			}
			return respuesta;
		}
		/// <summary>
		/// Método para consultar las incidencias relacionadas al usuario
		/// </summary>
		/// <param name="incidencias"></param>
		/// <returns></returns>
		public Respuesta<List<Incidencias>> ConsultarIncidenciasUsuario(int idUsuario)
		{
			Respuesta<List<Incidencias>> respuesta = new Respuesta<List<Incidencias>>();
			using (TransactionScope transaccion = new TransactionScope())
			{
				try
				{
					string SQL = @"EXEC Pa_ConsultarIncidenciasUsuario @IdUsuario";
					respuesta.ObjetoRespuesta = _contexto.Set<Entities.Incidencias>().
						FromSql(SQL,
					   new SqlParameter("@IdUsuario", idUsuario),
					_contexto.SaveChanges()).ToList();
					transaccion.Complete();
					respuesta.HayError = false;
				}
				catch (Exception ex)
				{
					transaccion.Dispose();
					respuesta.HayError = true;
					respuesta.MensajeError = ex.Message;
				}
			}
			return respuesta;
		}
		/// <summary>
		/// Método para cambiar el estado de la incidencia
		/// </summary>
		/// <param name="incidencias"></param>
		/// <returns></returns>
		public Respuesta<Incidencias> CambiarEstadoIncidencia(int idIncidencia, string estado)
		{
			Respuesta<Incidencias> respuesta = new Respuesta<Incidencias>();
			using (TransactionScope transaccion = new TransactionScope())
			{
				try
				{
					string SQL = @"EXEC Pa_CambiarEstadoIncidencia  @IdIncidencia, @EstadoIncidencia";
					respuesta.ObjetoRespuesta = _contexto.Set<Incidencias>().
						FromSql(SQL,
					   new SqlParameter("@IdIncidencia", idIncidencia),
					   new SqlParameter("@EstadoIncidencia", estado),
				   _contexto.SaveChanges()).FirstOrDefault();
					transaccion.Complete();
					respuesta.HayError = false;					
				}
				catch (Exception ex)
				{
					transaccion.Dispose();
					respuesta.HayError = true;
					respuesta.MensajeError = ex.Message;
				}
			}
			return respuesta;
		}
	}
}
