# Git Commands Cheat Sheet

## 📋 Comandos Básicos de Configuración

### `git config`
Configura Git con tu información personal
```bash
git config --global user.name "Tu Nombre"
git config --global user.email "tu@email.com"
git config --list                          # Ver toda la configuración
```

---

## 🚀 Inicialización y Clonado

### `git init`
Inicializa un nuevo repositorio Git en el directorio actual
```bash
git init
git init nombre-proyecto                    # Crea carpeta e inicializa
```

### `git clone`
Descarga un repositorio remoto completo
```bash
git clone https://github.com/usuario/repo.git
git clone https://github.com/usuario/repo.git mi-carpeta  # Con nombre personalizado
```

---

## 📝 Trabajando con Archivos

### `git status`
Muestra el estado actual de tu repositorio
```bash
git status                                  # Estado detallado
git status -s                              # Estado resumido
```

### `git add`
Agrega archivos al área de preparación (staging)
```bash
git add archivo.txt                         # Agregar archivo específico
git add .                                   # Agregar todos los archivos modificados
git add *.js                               # Agregar todos los archivos .js
git add carpeta/                           # Agregar toda una carpeta
```

### `git commit`
Guarda los cambios en el repositorio local
```bash
git commit -m "Mensaje descriptivo"        # Commit con mensaje
git commit -am "Mensaje"                   # Add + commit de archivos ya rastreados
git commit --amend -m "Nuevo mensaje"      # Modificar el último commit
```

---

## 🔍 Historial y Diferencias

### `git log`
Muestra el historial de commits
```bash
git log                                     # Historial completo
git log --oneline                          # Una línea por commit
git log --graph                           # Vista gráfica de ramas
git log -n 5                              # Últimos 5 commits
```

### `git diff`
Muestra las diferencias entre archivos
```bash
git diff                                    # Cambios no preparados
git diff --staged                          # Cambios preparados para commit
git diff HEAD~1                           # Diferencias con el commit anterior
```

---

## 🌿 Trabajando con Ramas (Branches)

### `git branch`
Gestiona ramas del repositorio
```bash
git branch                                  # Listar ramas locales
git branch nombre-rama                      # Crear nueva rama
git branch -d nombre-rama                   # Eliminar rama
git branch -a                              # Listar todas las ramas (locales y remotas)
```

### `git checkout`
Cambia entre ramas o restaura archivos
```bash
git checkout nombre-rama                    # Cambiar a una rama
git checkout -b nueva-rama                  # Crear y cambiar a nueva rama
git checkout -- archivo.txt                # Descartar cambios en archivo
```

### `git switch` (Git 2.23+)
Comando moderno para cambiar ramas
```bash
git switch nombre-rama                      # Cambiar a una rama
git switch -c nueva-rama                    # Crear y cambiar a nueva rama
```

### `git merge`
Fusiona ramas
```bash
git merge nombre-rama                       # Fusionar rama en la actual
git merge --no-ff nombre-rama              # Fusión sin fast-forward
```

---

## 🌐 Trabajando con Repositorios Remotos

### `git remote`
Gestiona conexiones a repositorios remotos
```bash
git remote                                  # Listar remotos
git remote -v                              # Listar remotos con URLs
git remote add origin https://github.com/user/repo.git  # Agregar remoto
git remote remove origin                    # Eliminar remoto
```

### `git pull`
**Descarga y fusiona cambios del repositorio remoto**
```bash
git pull origin main                        # Traer cambios de la rama main
git pull origin master                      # Traer cambios de la rama master
git pull                                    # Pull desde el remoto configurado
git pull --rebase origin main              # Pull con rebase en lugar de merge
```

### `git push`
Sube tus cambios al repositorio remoto
```bash
git push origin main                        # Subir a la rama main
git push origin nueva-rama                  # Subir nueva rama
git push -u origin main                     # Configurar upstream y subir
git push --force                           # Forzar push (¡cuidado!)
```

### `git fetch`
Descarga cambios sin fusionar
```bash
git fetch origin                            # Descargar cambios de origin
git fetch --all                           # Descargar de todos los remotos
```

---

## 🔄 Deshacer Cambios

### `git reset`
Deshace commits o cambios en staging
```bash
git reset archivo.txt                       # Quitar archivo del staging
git reset --soft HEAD~1                    # Deshacer último commit, mantener cambios
git reset --hard HEAD~1                    # Deshacer último commit y cambios
git reset --hard origin/main               # Resetear a la versión remota
```

### `git revert`
Crea un nuevo commit que deshace cambios anteriores
```bash
git revert HEAD                             # Revertir último commit
git revert abc1234                         # Revertir commit específico
```

### `git restore` (Git 2.23+)
Restaura archivos a versiones anteriores
```bash
git restore archivo.txt                     # Descartar cambios locales
git restore --staged archivo.txt           # Quitar del staging
git restore --source=HEAD~1 archivo.txt    # Restaurar a versión anterior
```

---

## 🏷️ Etiquetas (Tags)

### `git tag`
Marca puntos específicos en el historial
```bash
git tag                                     # Listar tags
git tag v1.0.0                             # Crear tag ligero
git tag -a v1.0.0 -m "Versión 1.0.0"      # Crear tag anotado
git push origin v1.0.0                     # Subir tag específico
git push origin --tags                     # Subir todos los tags
```

---

## 🔧 Comandos de Mantenimiento

### `git stash`
Guarda temporalmente cambios sin hacer commit
```bash
git stash                                   # Guardar cambios actuales
git stash pop                              # Aplicar último stash
git stash list                             # Ver lista de stash
git stash drop                             # Eliminar último stash
```

### `git clean`
Elimina archivos no rastreados
```bash
git clean -n                               # Ver qué se eliminaría (dry run)
git clean -f                               # Eliminar archivos no rastreados
git clean -fd                              # Eliminar archivos y directorios
```

---

## 📊 Comandos de Información

### `git show`
Muestra información detallada de commits
```bash
git show                                    # Mostrar último commit
git show abc1234                           # Mostrar commit específico
git show HEAD~2                            # Mostrar commit hace 2
```

### `git blame`
Muestra quién modificó cada línea de un archivo
```bash
git blame archivo.txt                       # Ver autoría línea por línea
```

---

## 🚨 Comandos de Emergencia

### Descartar todos los cambios locales
```bash
git reset --hard HEAD                      # Descartar cambios en tracked files
git clean -fd                              # Eliminar archivos no rastreados
```

### Sincronizar con remoto forzadamente
```bash
git fetch origin
git reset --hard origin/main               # ¡CUIDADO! Sobrescribe todo
```

### Ver qué archivos están siendo ignorados
```bash
git status --ignored                        # Ver archivos ignorados
git check-ignore archivo.txt               # Verificar si un archivo está ignorado
```

---

## 💡 Tips Útiles

1. **Siempre revisa antes de hacer push:**
   ```bash
   git status
   git log --oneline -5
   ```

2. **Para ver cambios antes de pull:**
   ```bash
   git fetch origin
   git log HEAD..origin/main --oneline
   ```

3. **Configurar editor por defecto:**
   ```bash
   git config --global core.editor "code --wait"  # Para VS Code
   ```

4. **Crear alias útiles:**
   ```bash
   git config --global alias.st status
   git config --global alias.co checkout
   git config --global alias.br branch
   ```

---

## 📚 Flujo de Trabajo Típico

```bash
# 1. Clonar o actualizar repositorio
git clone https://github.com/usuario/repo.git
# o
git pull origin main

# 2. Crear nueva rama para feature
git checkout -b nueva-funcionalidad

# 3. Hacer cambios y commits
git add .
git commit -m "Agregar nueva funcionalidad"

# 4. Subir rama
git push -u origin nueva-funcionalidad

# 5. Merge (normalmente via Pull Request)
git checkout main
git pull origin main
git merge nueva-funcionalidad

# 6. Limpiar
git branch -d nueva-funcionalidad
git push origin --delete nueva-funcionalidad
```