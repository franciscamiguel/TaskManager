import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  private baseUrl = 'https://localhost:7197/api/v1/files';

  constructor(private http: HttpClient) { }

  /**
   * Faz upload de um arquivo
   * @param file 
   * @param entityId 
   * @param folderPathName 
   * @returns 
   */
  uploadFile(file: FormData, entityId: number, folderPathName: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/upload/${entityId}`, file, { params: { folderPathName } });
  }

  /**
   * Atualiza um arquivo
   * @param file 
   * @param entityId 
   * @param folderPathName 
   * @param oldKey 
   * @returns 
   */
  updateFile(file: FormData, entityId: number, folderPathName: string, oldKey: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/update-attachment/${entityId}`, file, { params: { folderPathName, oldKey } });
  }

  /**
   * Consulta a imagem no S3 para visualizar
   * @param key 
   * @returns 
   */
  downloadFile(key: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/download`, { params: {key: key}, responseType: 'blob' });
  }

  /**
   * Excluir um aquivo
   * @param key 
   * @returns 
   */
  deleteFile(key: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/delete/${key}`);
  }

  /**
   * exclui arquivos em massa
   * @param keys 
   * @returns 
   */
  deleteMultipleFiles(keys: string[]): Observable<any> {
    return this.http.post(`${this.baseUrl}/delete-multiple`, keys);
  }
}
