import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../../services/tasks/task.service';
import { Task, TaskDto } from '../../../models/task.model';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { FileService } from '../../../services/files/file.service';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent implements OnInit {
  title = 'Tarefas';
  tasks: Task[] = [];
  _filterList!: string;
  bodyDeleteTask: any;

  task: Task | null = null;
  taskDto: TaskDto | null = null;

  tasksFiltered: any = [];
  registerForm!: FormGroup;
  selectedTaskId: number | null = null;

  constructor(
    private taskService: TaskService,
    private fileService: FileService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private  spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.spinner.show();

    this.validation();
    this.getTasks();
  }

  validation() {
    this.registerForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      description: ['', Validators.required, Validators.minLength(3)],
    });
  }

  getTasks(): void {
    this.taskService.getTasks(1, 500).subscribe({
      next: (result: any) => {
        this.tasks = result.data.data;
        this.tasksFiltered = result.data.data;
      }, 
      error: (error: any) => {
        this.spinner.hide();
        const toast = this.toastr.error(`Erro ao obter tarefas: ${JSON.stringify(error)}`);
        console.log(JSON.stringify(error.error));
        console.log(toast.message);
      }, complete: () => {
        return this.spinner.hide();
      }
    })
  }

  deleteTask(id: number): void {
    this.taskService.deleteTask(id).subscribe(response => {
      console.log('Tarefa excluída com sucesso:', response);
      this.getTasks();
    });
  }
  
  viewFile(attachmentKey: string): void {
    this.fileService.downloadFile(attachmentKey).subscribe({
      next: (fileBlob) => {
        const url = window.URL.createObjectURL(fileBlob);
        window.open(url);
      },
      error: (error) => {
        this.toastr.error('Erro ao abrir o arquivo.');
      }
    });
  }
}