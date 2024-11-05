import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, TaskDto } from '../../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private baseUrl = 'https://localhost:7197/api/v1/tasks';

  constructor(private http: HttpClient) { }

  /**
   * Consulta as tarefas
   * @param pageNumber 
   * @param pageSize 
   * @returns 
   */
  getTasks(pageNumber: number, pageSize: number): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.baseUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  /**
   * Consulta uma tarefa
   * @param id 
   * @returns 
   */
  getTask(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.baseUrl}/${id}`);
  }

  /**
   * Cria um novo registro da tarefa
   * @param task 
   * @returns 
   */
  createTask(task: TaskDto): Observable<any> {
    return this.http.post(`${this.baseUrl}`, task);
  }

  /**
   * Atualiza uma tarefa
   * @param id 
   * @param task 
   * @returns 
   */
  updateTask(id: number, task: TaskDto): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, task);
  }

  /**
   * Exclui uma tarefa
   * @param id 
   * @returns 
   */
  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
