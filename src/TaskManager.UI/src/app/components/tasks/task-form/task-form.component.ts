import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../../services/tasks/task.service';
import { Task, TaskDto } from '../../../models/task.model';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { FileService } from '../../../services/files/file.service';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {
  taskForm!: FormGroup;
  taskId: number | null = null;
  isEditMode = false;
  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private fileService: FileService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.spinner.show();
    this.initializeForm();

    // Verifica se estamos editando uma tarefa
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.taskId = +id;
        this.isEditMode = true;
        this.loadTask();
      } else {
        this.isEditMode = false;
        this.spinner.hide();
      }
    });
  }

  initializeForm(): void {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.minLength(3)]],
      isCompleted: [false],
      attachment: [null]
    });
  }

  loadTask(): void {
    if (this.taskId) {
      this.taskService.getTask(this.taskId).subscribe({
        next: (result: any) => {
          this.taskForm.patchValue(result.data);
          this.spinner.hide();
        },
        error: (error: any) => {
          this.toastr.error('Erro ao carregar a tarefa.');
          this.spinner.hide();
        }
      });
    }
  }

  saveTask(): void {
    if (this.taskForm.valid) {
      const taskDto: TaskDto = this.taskForm.value;

      if (this.isEditMode && this.taskId) {
        this.taskService.updateTask(this.taskId, taskDto).subscribe({
          next: (result) => {
            this.toastr.success('Tarefa atualizada com sucesso.');
          
            if(result.success) this.uploadFile(result.data);
          },
          error: () => this.toastr.error('Erro ao atualizar a tarefa.')
        });
      } else {
        this.taskService.createTask(taskDto).subscribe({
          next: (result) => {
            this.toastr.success('Tarefa criada com sucesso.');

            if(result.success) this.uploadFile(result.data);
          },
          error: () => this.toastr.error('Erro ao criar a tarefa.')
        });
      }
    }
  }

  uploadFile(taskId: number): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('File', this.selectedFile);  // Use o nome 'File' conforme o backend espera
      formData.append('taskId', taskId.toString());  // Adiciona o ID da tarefa
  
      this.fileService.uploadFile(formData, taskId, 'tasks/').subscribe({
        next: () => {
          this.toastr.success('Arquivo enviado com sucesso.');
          this.router.navigate(['/']);
        },
        error: () => {
          this.toastr.error('Erro ao enviar o arquivo.');
        }
      });
    }
  }  
  
  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;  // Armazena o arquivo selecionado
    }
  }
}
