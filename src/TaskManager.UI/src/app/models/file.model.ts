export interface FileDto {
    file: File; // O arquivo físico que será enviado
  }
  
  export interface FileResponse {
    fileName: string; // Nome do arquivo no S3
    message: string; // Mensagem de resposta
  }
  